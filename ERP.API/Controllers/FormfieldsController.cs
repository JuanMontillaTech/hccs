
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class FormfieldsController : ControllerBase
    {
        private readonly IGenericRepository<Formfields> _repFormfields;
        private readonly IGenericRepository<Section> _repositorySection;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FormfieldsController(IGenericRepository<Formfields> repFormfields, IGenericRepository<Section> repositorySection, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repFormfields = repFormfields;
            _repositorySection = repositorySection;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormfieldsDto data)
        {
            var mapper = _mapper.Map<Formfields>(data);


            var result = await _repFormfields.Insert(mapper);
            try
            {
                var DataSave = await _repFormfields.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<FormfieldsDto>(result);
                return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<FormfieldsDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await _repFormfields.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Froms).ToListAsync();
             

            var mapperOut = _mapper.Map<FormfieldsDetallisDto[]>(DataSave);

            return Ok(Result<FormfieldsDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await _repFormfields.GetById(id);

            var mapperOut = _mapper.Map<FormfieldsDto>(DataSave);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        
        
        
         
        [HttpGet("GetSectionWithFildsByFormID/{id}")]
        public async Task<IActionResult> GetSectionWithFildsByFormID( Guid id)
        {
            try
            {

                var dataSection = await _repositorySection.GetAll();

                var sectionFieldsDto = new List<SectionFieldsDto>();
                foreach (var section in dataSection)
                {
                    var sectionNewRow = new SectionFieldsDto();
                    sectionNewRow.Name = section.Name.Trim();
            
                    var formfields = await _repFormfields.Find(x => x.IsActive &&
                                                                    x.FormId == id && 
                                                                    x.SectionId == section.Id).AsQueryable()
                        .Include(x => x.Froms).OrderBy(x => x.Index).ToListAsync();

         
                    if (formfields.Count > 0)
                    {
                        sectionNewRow.Fields = _mapper.Map<List<FormfieldsDetallisDto>>(formfields);
                        sectionFieldsDto.Add(sectionNewRow);
                    }

                }
        
        
        
     
                return Ok(Result<List<SectionFieldsDto>>.Success(sectionFieldsDto, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<FormfieldsDetallisDto[]>.Fail(MessageCodes.BabData(),ex.Message));
            }
        }

        [HttpGet("GetByFormId/{id}")]
        public async Task<IActionResult> GetByFormId( Guid id)
        {
            try
            {

              
                
                
                
                var DataSave = await _repFormfields.Find(x => x.IsActive && x.FormId == id).AsQueryable()
                    .Include(x => x.Froms).Include(x=> x.Section).OrderBy(x => x.Index).ToListAsync();

                
                
                var mapperOut = _mapper.Map<FormfieldsDetallisDto[]>(DataSave);

                return Ok(Result<FormfieldsDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<FormfieldsDetallisDto[]>.Fail(MessageCodes.BabData(),ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await _repFormfields.GetById(id);

            Data.IsActive = false;

            await _repFormfields.Update(Data);

            var save = await _repFormfields.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormfieldsDto>(Data);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormfieldsDto _UpdateDto)
        {

            var mapper = _mapper.Map<Formfields>(_UpdateDto);
            mapper.IsActive = true;
            var result = await _repFormfields.Update(mapper);

            var DataSave = await _repFormfields.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormfieldsDto>(result);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
