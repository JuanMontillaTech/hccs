﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class FormLedgerAccountController : ControllerBase
    {
        private readonly IGenericRepository<FormLedgerAccount> _repFormLedger;
        private readonly IGenericRepository<LedgerAccount> _repLedgerAccounts;

        private readonly IMapper _mapper;

        private int _dataSave;

        public FormLedgerAccountController(IGenericRepository<FormLedgerAccount> repFormLedger, IGenericRepository<LedgerAccount> repLedgerAccounts, IMapper mapper)
        {
            _repFormLedger = repFormLedger;
            _repLedgerAccounts = repLedgerAccounts;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormLedgerAccountDto data)
        {

            var existeFormLAger = await _repFormLedger.Find(x => x.IsActive
                 && x.LedgerAccountId == data.LedgerAccountId && x.FormId == data.FormId).AsQueryable()
                //.Include(x => x.LedgerAccount)
                .ToListAsync();
            if (existeFormLAger.Count > 0)
                return Ok(Result<FormLedgerAccountDto>.Fail("la cuenta existe para este formulario", MessageCodes.BabData()));



            var mapper = _mapper.Map<FormLedgerAccount>(data);
            mapper.Index = data.Index == null ? 0 : data.Index;

            var result = await _repFormLedger.InsertAsync(mapper);

            _dataSave = await _repFormLedger.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<FormLedgerAccountDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<FormLedgerAccountDto>(result);
            return Ok(Result<FormLedgerAccountDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repFormLedger.Find(x => x.IsActive).AsQueryable()
                //.Include(x => x.LedgerAccount)
                .ToListAsync();


            var mapperOut = _mapper.Map<FormLedgerAccountDto[]>(dataSave);

            return Ok(Result<FormLedgerAccountDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BankDetallisDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getFormLedger = _repFormLedger.Find(x => x.IsActive == true
                && (x.LedgerAccount.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
                || (x.Forms.Title.ToLower().Contains(filter.Search.Trim().ToLower()))
                ).Where(x => x.IsActive == true).Take(filter.PageSize)
                .Include(x => x.Forms)
                 .Include(x => x.LedgerAccount)
                .ToList();

            int totalRecords = getFormLedger.Count();
            var dataMaperOut = _mapper.Map<List<FormLedgerAccountDto>>(getFormLedger);

            var listFormLedger = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<FormLedgerAccountDto>>.Success(listFormLedger);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repFormLedger.Find(x => x.IsActive == true && x.Id == id)
                .Include(x => x.Forms)
                .Include(x => x.LedgerAccount)
                .FirstOrDefaultAsync();

            var mapperOut = _mapper.Map<FormLedgerAccountDto>(dataSave);

            return Ok(Result<FormLedgerAccountDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetByFormId")]
        public async Task<IActionResult> GetByFormId([FromQuery] Guid formId)
        {

            var formLedgerAccounts = await _repFormLedger.Find(x => x.IsActive == true && x.FormId == formId)
              .OrderBy(x => x.CreatedDate).ToListAsync();


            var ledgerAccountIds = formLedgerAccounts.Select(fl => fl.LedgerAccountId).ToList();

            var ledgerAccounts = new List<LedgerAccount>();
         
                foreach (var ledgerAccountId in ledgerAccountIds)
                {
                    var ledgerAccount = await _repLedgerAccounts.GetById(ledgerAccountId);
                    if (ledgerAccount != null)
                    {
                        ledgerAccounts.Add(ledgerAccount);
                    }
                }

              
           

          
            var ledgerAccountDtos = _mapper.Map<List<LedgerAccountDto>>(ledgerAccounts);

            return Ok(Result<List<LedgerAccountDto>>.Success(ledgerAccountDtos, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetByFormIdHccs")]
        public async Task<IActionResult> GetByFormIdHccs([FromQuery] Guid formId)
        {

            var formLedgerAccounts = await _repFormLedger.Find(x => x.IsActive == true && x.FormId == formId)
                .Include(x=>x.LedgerAccount)
                .OrderBy(x => x.CreatedDate).ToListAsync(); 

            return Ok(Result< List<FormLedgerAccount>>.Success(formLedgerAccounts, MessageCodes.AllSuccessfully()));

           
        }


        [HttpGet("GetByFormIdYear")]
        public async Task<IActionResult> GetByFormIdYear([FromQuery] Guid formId, int year)
        {
            var ledgerAccounts = await _repFormLedger.Find(x => x.IsActive == true && x.FormId == formId)
                .Include(x => x.LedgerAccount)
               .Where(x => x.LedgerAccount.EntidadId == year)
                .OrderBy(x => x.CreatedDate)
                .Select(x => new LedgerAccountIndexDto 
                {
                    Id = x.LedgerAccountId,
                    Belongs = x.LedgerAccount.Belongs,
                    Name = x.LedgerAccount.Name,
                    Code = x.LedgerAccount.Code,
                    Nature = x.LedgerAccount.Nature.HasValue ? x.LedgerAccount.Nature.Value : 0,
                    LocationStatusResult = x.LedgerAccount.LocationStatusResult.HasValue ? x.LedgerAccount.LocationStatusResult.Value : 0,
                    Index = x.Index.HasValue ? x.Index.Value : 0 
                })
                .ToListAsync();

            return Ok(Result<List<LedgerAccountIndexDto>>.Success(ledgerAccounts, MessageCodes.AllSuccessfully()));
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repFormLedger.GetById(id);

            data.IsActive = false;

            await _repFormLedger.Update(data);

            var save = await _repFormLedger.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormLedgerAccountDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormLedgerAccountDto>(data);

            return Ok(Result<FormLedgerAccountDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormLedgerAccountDto updateDto)
        {
            var existeFormLAger = await _repFormLedger.Find(x=> x.Id == updateDto.Id).AsQueryable()
        
                .FirstOrDefaultAsync();

            if (existeFormLAger == null)
                return Ok(Result<FormLedgerAccountDto>.Fail("la cuenta no existe para este formulario", MessageCodes.BabData()));

            var mapper = _mapper.Map<FormLedgerAccount>(updateDto);
      
            existeFormLAger.IsActive = true; 
            existeFormLAger.FormId = updateDto.FormId;
            existeFormLAger.LedgerAccountId = updateDto.LedgerAccountId;
            existeFormLAger.Index = updateDto.Index == null ? 0 : updateDto.Index;
            
            var result = await _repFormLedger.Update(existeFormLAger);

            var dataSave = await _repFormLedger.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<FormLedgerAccountDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormLedgerAccountDto>(result);

            return Ok(Result<FormLedgerAccountDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}