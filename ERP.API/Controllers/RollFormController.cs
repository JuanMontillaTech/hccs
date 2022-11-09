
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
    public class RollFormController : ControllerBase
    {
        private readonly IGenericRepository<RollForm> _repRollForm;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RollFormController(IGenericRepository<RollForm> repRollForm, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repRollForm = repRollForm;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RollFormDto data)
        {
            var mapper = _mapper.Map<RollForm>(data);


            var result = await _repRollForm.Insert(mapper);
            try
            {
                var dataSave = await _repRollForm.SaveChangesAsync();
                if (dataSave != 1)
                    return Ok(Result<RollFormDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<RollFormDto>(result);
                return Ok(Result<RollFormDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<RollFormDto>.Fail("Error al insertar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repRollForm.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Froms).Include(x=> x.Rolles).ToListAsync();
             

            var mapperOut = _mapper.Map<RollFormDetallisDto[]>(dataSave);

            return Ok(Result<RollFormDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<RollFormDetallisDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repRollForm.Find(x => x.IsActive == true
            && (x.Froms.Title.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Rolles.Name.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).Include(x => x.Froms).Include(x => x.Rolles).ToList();

            int totalRecords = _repRollForm.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<RollFormDetallisDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<RollFormDetallisDto>>.Success(List);
            return Ok(Result);

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repRollForm.GetById(id);

            var mapperOut = _mapper.Map<RollFormDto>(dataSave);

            return Ok(Result<RollFormDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repRollForm.GetById(id);

            data.IsActive = false;

            await _repRollForm.Update(data);

            var save = await _repRollForm.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<RollFormDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<RollFormDto>(data);

            return Ok(Result<RollFormDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] RollFormDto updateDto)
        {

            var mapper = _mapper.Map<RollForm>(updateDto);
            mapper.IsActive = true;
            var result = await _repRollForm.Update(mapper);

            var dataSave = await _repRollForm.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<RollFormDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<RollFormDto>(result);

            return Ok(Result<RollFormDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
