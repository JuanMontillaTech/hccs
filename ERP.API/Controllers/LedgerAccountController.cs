using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class LedgerAccountController : ControllerBase
    {
        public readonly IGenericRepository<LedgerAccount> RepLedgerAccounts;

        private readonly IMapper _mapper;
        public LedgerAccountController(IGenericRepository<LedgerAccount> repLedgerAccounts, IMapper mapper)
        {
            RepLedgerAccounts = repLedgerAccounts;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LedgerAccountDto data)
        {
            var mapper = _mapper.Map<LedgerAccount>(data);

            var result = await RepLedgerAccounts.InsertAsync(mapper);

            var DataSave = await RepLedgerAccounts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<LedgerAccountDto>(result);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        private async Task<IActionResult> getAccountByCode(string Code)
        {
            var DataSave = await RepLedgerAccounts.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true && x.Code == Code).OrderBy(x=> x.Name).ToList();

            var mapperOut = _mapper.Map<LedgerAccountDto[]>(Filter);

            return Ok(Result<LedgerAccountDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepLedgerAccounts.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<LedgerAccountDto[]>(Filter);

            return Ok(Result<LedgerAccountDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<LedgerAccountDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
       {

            var Filter = RepLedgerAccounts.Find(x => x.IsActive == true
            && (x.Code.ToLower().Contains(filter.Search.Trim().ToLower()))
          || (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).Where(x=> x.IsActive == true).Take(filter.PageSize).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<LedgerAccountDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<LedgerAccountDto>>.Success(List);
            return Ok(Result);

        }  
        [HttpGet("GetFormHSS")]
        [ProducesResponseType(typeof(Result<ICollection<LedgerAccountDto>>), (int)HttpStatusCode.OK)]

  



        [HttpGet("GetPanrentWithChildrenAll")]
        public async Task<IActionResult> GetPanrentWithChildrenAll()
        {
            LegarUtil legarUtil = new LegarUtil();
            var BaseData = await RepLedgerAccounts.GetAll();
            var AllParents = BaseData;
            List<AccountDTtos> accountDTtos = new List<AccountDTtos>();
            foreach (var Parentsrow in AllParents.Where(x => x.IsActive == true && x.Belongs == null).ToList())
            {
                AccountDTtos ParentNew = new AccountDTtos();
                ParentNew.Id = Parentsrow.Id;
                ParentNew.Text = Parentsrow.Name;
                ParentNew.Description = Parentsrow.Commentary;
                ParentNew.AccountCode = Parentsrow.Code;
                var baseChildrens1 = BaseData;
                var Childrens = legarUtil.GetChildrend(ParentNew, baseChildrens1.ToList());
                if(Childrens.Id != Guid.Empty)
                ParentNew.Children.Add(Childrens);
                accountDTtos.Add(ParentNew);
            } 
            return Ok(Result<List<AccountDTtos>>.Success(accountDTtos, MessageCodes.AllSuccessfully()));
        } 

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepLedgerAccounts.GetById(id);

            var mapperOut = _mapper.Map<LedgerAccountDto>(DataSave);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepLedgerAccounts.GetById(id);

            Data.IsActive = false;

            await RepLedgerAccounts.Update(Data);

            var save = await RepLedgerAccounts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<LedgerAccountDto>(Data);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LedgerAccountDto _UpdateDto)
        {
            var mapper = _mapper.Map<LedgerAccount>(_UpdateDto);

            var result = await RepLedgerAccounts.Update(mapper);

            result.IsActive = true;

            var DataSave = await RepLedgerAccounts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<LedgerAccountDto>(result);

            return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
