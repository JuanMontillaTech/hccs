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
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IGenericRepository<Banks> _repBanks;

        private readonly IMapper _mapper;
   
        private int _dataSave;

        public BankController(IGenericRepository<Banks> repBanks, IMapper mapper)
        {
            _repBanks = repBanks;
        
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankDto data)
        {

        var mapper = _mapper.Map<Banks>(data);


            var result = await _repBanks.InsertAsync(mapper);
       
                _dataSave = await _repBanks.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<BankDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<BankDto>(result);
                return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
          
           
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repBanks.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Currencys).Include(x => x.LedgerAccount).ToListAsync();


            var mapperOut = _mapper.Map<BankDetallisDto[]>(dataSave);

            return Ok(Result<BankDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BankDetallisDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getBanks = _repBanks.Find(x => x.IsActive == true
            && (x.AccountNumber.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repBanks.Find(t => t.IsActive).Count();
            var dataMaperOut = _mapper.Map<List<BankDetallisDto>>(getBanks);

            var listBanks = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords) ;
            var result = Result<PagesPagination<BankDetallisDto>>.Success(listBanks);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repBanks.GetById(id);

            var mapperOut = _mapper.Map<BankDto>(dataSave);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repBanks.GetById(id);

            data.IsActive = false;

            await _repBanks.Update(data);

            var save = await _repBanks.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<BankDto>(data);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankDto updateDto)
        {
            var mapper = _mapper.Map<Banks>(updateDto);
            mapper.IsActive = true;
            var result = await _repBanks.Update(mapper);

            var dataSave = await _repBanks.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<BankDto>(result);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}