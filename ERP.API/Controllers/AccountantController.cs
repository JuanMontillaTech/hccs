//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ERP.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountantController : ControllerBase
//    {
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly IGenericRepository<LedgerAccount> _repLedgerAccount;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public AccountantController(IGenericRepository<LedgerAccount> repLedgerAccount, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repLedgerAccount = repLedgerAccount;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LedgerAccountDto data)
        {
            var mapper = _mapper.Map<LedgerAccount>(data);


            var result = await _repLedgerAccount.Insert(mapper);
            try
            {
                _dataSave = await _repLedgerAccount.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<LedgerAccountDto>(result);
                return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<LedgerAccountDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repLedgerAccount.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<LedgerAccountDto[]>(dataSave);

            return Ok(Result<LedgerAccountDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<LedgerAccountDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repLedgerAccount.Find(x => x.IsActive == true
            && (x.Code.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repLedgerAccount.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<LedgerAccountDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<LedgerAccountDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repLedgerAccount.GetById(id);

            var mapperOut = _mapper.Map<LedgerAccountDto>(dataSave);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repLedgerAccount.GetById(id);

            data.IsActive = false;

            await _repLedgerAccount.Update(data);

            var save = await _repLedgerAccount.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<LedgerAccountDto>(data);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LedgerAccountDto updateDto)
        {
            var mapper = _mapper.Map<LedgerAccount>(updateDto);
            mapper.IsActive = true;
            var result = await _repLedgerAccount.Update(mapper);

            var dataSave = await _repLedgerAccount.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<LedgerAccountDto>(result);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}
