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
using System.Linq;
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
        private readonly ICurrentUser currentUser;
        public FileManagerController(IGenericRepository<FileManager> _RepFileManager,
            IAWS _aWS ,
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor
             , ICurrentUser currentUser
            )
        {
            RepFileManager = _RepFileManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            aWS = _aWS;
            this.currentUser = currentUser;

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
                fileout.link = $"http://montillasoft.{currentUser.DataBaseName().ToString().ToLower()}.s3.amazonaws.com/{DataSave.PhysicalName}";

            }
            

            return Ok(Result<FileLinkDto>.Success(fileout, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetAllBySourceId")]
        public async Task<IActionResult> GetAllBySourceId([FromQuery] Guid SourceId)
        {
            List<FileLinkDto> fileLinkList = new List<FileLinkDto>();
            var DataSave = await RepFileManager.Find(x => x.SourceId == SourceId && x.IsActive == true ).OrderByDescending(x=> x.CreatedDate).ToListAsync();
           
            
            if (DataSave != null)
            {
                foreach (var item in DataSave)
                {
                    var fileout = new FileLinkDto();

                    fileout.Id = item.Id;
                    fileout.Name = item.PhysicalName;
                    fileout.SourceId = SourceId;
                    fileout.link = $"http://montillasoft.{currentUser.DataBaseName()}.s3.amazonaws.com/{item.PhysicalName}";
                    fileLinkList.Add(fileout);
                }

            }


            return Ok(Result<List<FileLinkDto>>.Success(fileLinkList, MessageCodes.AllSuccessfully()));
        }
        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id) 
        {
         
            var DataSave = await RepFileManager.GetById(id);


           
                DataSave.IsActive = false;

                await RepFileManager.Update(DataSave);

                var save = await RepFileManager.SaveChangesAsync();

                if (save != 1)
                    return Ok(Result<FileManagerDto>.Fail(MessageCodes.ErrorDeleting, "API"));

                var mapperOut = _mapper.Map<FileManagerDto>(DataSave);

                return Ok(Result<FileManagerDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));

 


           
        }

        [HttpPost("UploadFiles")]
        public Task<IActionResult> UploadFiles([FromForm] ICollection<IFormFile> request,[FromForm] Guid reccordId)
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
                                            SourceId = reccordId,
                                            PhysicalName = file.FileName,
                                            Folder ="bucket",
                                        };
                                    RepFileManager.Insert(files);
                                    RepFileManager.Save();
                      



                    }
                 

                }


            }
           

            return Task.FromResult<IActionResult>(Ok(Result<bool>.Success(true, MessageCodes.AddedSuccessfully())));

        }

    }
}
