 

using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Model.Dtos;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class FormGridController : ControllerBase
    {
        private readonly IGenericRepository<FormGrid> _repFormGrid;
        private readonly IGenericRepository<Section> _repositorySection;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FormGridController(IGenericRepository<FormGrid> repFormGrid, IGenericRepository<Section> repositorySection, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repFormGrid = repFormGrid;
            _repositorySection = repositorySection;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormGridDto data)
        {
            var mapper = _mapper.Map<FormGrid>(data);


            var result = await _repFormGrid.InsertAsync(mapper);
            try
            {
                var DataSave = await _repFormGrid.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<FormGridDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<FormGridDto>(result);
                return Ok(Result<FormGridDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<FormGridDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await _repFormGrid.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Froms).ToListAsync();


            var mapperOut = _mapper.Map<FormGridDtoDetallisDto[]>(DataSave);

            return Ok(Result<FormGridDtoDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<FormGridDtoDetallisDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repFormGrid.Find(x => x.IsActive == true
            && (x.Field.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Label.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = _repFormGrid.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<FormGridDtoDetallisDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<FormGridDtoDetallisDto>>.Success(List);
            return Ok(Result);

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await _repFormGrid.GetById(id);

            var mapperOut = _mapper.Map<FormGridDto>(DataSave);

            return Ok(Result<FormGridDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }




        [HttpGet("GetSectionWithFildsByFormID/{id}")]
        public async Task<IActionResult> GetSectionWithFildsByFormID(Guid id)
        {
            try
            {

                var dataSection = await _repositorySection.Find(x=> x.IsActive == true)
                    .OrderBy(x=> x.Index).ToListAsync();

                var sectionFieldsDto = new List<SectionFormGrid>();
                foreach (var section in dataSection)
                {
                    var sectionNewRow = new SectionFormGrid();
                    sectionNewRow = _mapper.Map<SectionFormGrid>(section);


                    var FormGrid = await _repFormGrid.Find(x => x.IsActive &&
                                                                    x.FormId == id &&
                                                                    x.SectionId == section.Id).AsQueryable()
                        .Include(x => x.Froms).OrderBy(x => x.Index).ToListAsync();


                    if (FormGrid.Count > 0)
                    {
                        sectionNewRow.Fields = _mapper.Map<List<FormGridDtoDetallisDto>>(FormGrid);
                        sectionFieldsDto.Add(sectionNewRow);
                    }

                }




                return Ok(Result<List<SectionFormGrid>>.Success(sectionFieldsDto, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<FormGridDtoDetallisDto[]>.Fail(MessageCodes.BabData(), ex.Message));
            }
        }

        [HttpGet("GetByFormId/{id}")]
        public async Task<IActionResult> GetByFormId(Guid id)
        {
            try
            {





                var DataSave = await _repFormGrid.Find(x => x.IsActive && x.FormId == id).AsQueryable()
                    .Include(x => x.Froms).Include(x => x.Section).OrderBy(x => x.Index).ToListAsync();



                var mapperOut = _mapper.Map<FormGridDtoDetallisDto[]>(DataSave);

                return Ok(Result<FormGridDtoDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<FormGridDtoDetallisDto[]>.Fail(MessageCodes.BabData(), ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await _repFormGrid.GetById(id);

            Data.IsActive = false;

            await _repFormGrid.Update(Data);

            var save = await _repFormGrid.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormGridDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormGridDto>(Data);

            return Ok(Result<FormGridDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormGridDto _UpdateDto)
        {

            var mapper = _mapper.Map<FormGrid>(_UpdateDto);
            mapper.IsActive = true;
            var result = await _repFormGrid.Update(mapper);

            var DataSave = await _repFormGrid.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormGridDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormGridDto>(result);

            return Ok(Result<FormGridDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
