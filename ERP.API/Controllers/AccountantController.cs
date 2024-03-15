using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants; 
using ERP.Domain.Entity;
using ERP.Domain.Filter; 
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
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
     
        private int _dataSave;

        public AccountantController(IGenericRepository<LedgerAccount> repLedgerAccount, IMapper mapper
            )
        {
            _repLedgerAccount = repLedgerAccount;
          
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LedgerAccountDto data)
        {
            var mapper = _mapper.Map<LedgerAccount>(data);


            var result = await _repLedgerAccount.InsertAsync(mapper);
          
                _dataSave = await _repLedgerAccount.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<LedgerAccountDto>(result);
                return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
           

           
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

            var getAccount = _repLedgerAccount.Find(x => x.IsActive == true
            && x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
             || x.Name.ToLower().Contains(filter.Search.Trim().ToLower())
            ).ToList();

            var totalRecords = getAccount.Count();
            var dataMaperOut = _mapper.Map<List<LedgerAccountDto>>(getAccount);

            var list = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<LedgerAccountDto>>.Success(list);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repLedgerAccount.GetById(id);

            var mapperOut = _mapper.Map<LedgerAccountDto>(dataSave);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id:guid}")]
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
