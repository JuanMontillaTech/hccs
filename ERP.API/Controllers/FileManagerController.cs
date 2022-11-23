using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Implementations;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        public readonly IGenericRepository<FileManager> RepFileManager;
        public  readonly IAWS aWS;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileManagerController(IGenericRepository<FileManager> _RepFileManager, IAWS _aWS ,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepFileManager = _RepFileManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            aWS = _aWS;

        }

        [HttpGet("GetBySourceIdFirst")]
        public async Task<IActionResult> GetById([FromQuery] Guid SourceId)
        {
            var DataSave = await RepFileManager.Find(x=> x.SourceId == SourceId).FirstOrDefaultAsync();
            var fileout = new FileLinkDto();
            if (DataSave !=null)
            {
                fileout.Id = DataSave.Id;
                fileout.Name = DataSave.PhysicalName;
                fileout.SourceId = SourceId;
                fileout.link = $"https://perrisimo.s3.amazonaws.com/{DataSave.PhysicalName}";

            }
            

            return Ok(Result<FileLinkDto>.Success(fileout, MessageCodes.AllSuccessfully()));
        }

        [HttpPost("UploadFiles")]
        public     async Task<IActionResult> UploadFiles([FromForm] ICollection<IFormFile> request,[FromForm] Guid ReccordID)
        {


            List<FileManager> FileLoads = new List<FileManager>();
       
            foreach (var file in request)
            {
                if (file.Length > 0)
                {
                   

                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        var filems = new System.IO.MemoryStream(fileBytes);
                      
                        var resultUpload = Task.FromResult(aWS.WriteObject(filems, "test", file.FileName));
                   FileManager files = new FileManager()
                                        {
                                            ContentType = file.ContentType,
                                            DisplayName = file.FileName,
                                            SourceId = ReccordID,
                                            PhysicalName = file.FileName,
                                            Folder ="bucket",
                                        };
                                    RepFileManager.Insert(files);
                                    RepFileManager.Save();
                      



                    }
                 

                }


            }
           

            return Ok(Result<bool>.Success(true, MessageCodes.AddedSuccessfully()));

        }

    }
}
