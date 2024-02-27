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
    public class LedgerAccountCategoryController : ControllerBase
    {
        private readonly IGenericRepository<LedgerAccountCategory> _repCategory;
        
        private readonly IMapper _mapper;
   
        private int _dataSave;

        public LedgerAccountCategoryController(IGenericRepository<LedgerAccountCategory> repCategory, IMapper mapper)
        {
            _repCategory = repCategory;
        
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankDto data)
        {

        var mapper = _mapper.Map<LedgerAccountCategory>(data);


            var result = await _repCategory.InsertAsync(mapper);
       
                _dataSave = await _repCategory.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<LedgerAccountCategoryDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<LedgerAccountCategoryDto>(result);
                return Ok(Result<LedgerAccountCategoryDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
          
           
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repCategory.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<LedgerAccountCategoryDto[]>(dataSave);

            return Ok(Result<LedgerAccountCategoryDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<LedgerAccountCategoryDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getBanks = _repCategory.Find(x => x.IsActive == true
             && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repCategory.Find(t => t.IsActive).Count();
            var dataMaperOut = _mapper.Map<List<LedgerAccountCategoryDto>>(getBanks);

            var listBanks = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords) ;
            var result = Result<PagesPagination<LedgerAccountCategoryDto>>.Success(listBanks);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repCategory.GetById(id);

            var mapperOut = _mapper.Map<LedgerAccountCategoryDto>(dataSave);

            return Ok(Result<LedgerAccountCategoryDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repCategory.GetById(id);

            data.IsActive = false;

            await _repCategory.Update(data);

            var save = await _repCategory.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<LedgerAccountCategoryDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<LedgerAccountCategoryDto>(data);

            return Ok(Result<LedgerAccountCategoryDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LedgerAccountCategoryDto updateDto)
        {
            var mapper = _mapper.Map<LedgerAccountCategory>(updateDto);
            mapper.IsActive = true;
            var result = await _repCategory.Update(mapper);

            var dataSave = await _repCategory.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<LedgerAccountCategoryDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<LedgerAccountCategoryDto>(result);

            return Ok(Result<LedgerAccountCategoryDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}