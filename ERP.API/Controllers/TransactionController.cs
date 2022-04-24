
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly IGenericRepository<Transactions> RepTransactionss;
        public readonly IGenericRepository<TransactionsDetails> RepTransactionssDetails;
        public readonly INumerationService numerationService;


        private readonly IMapper _mapper;
        public TransactionController(IGenericRepository<Transactions> repTransactionss,
        IGenericRepository<TransactionsDetails> repTransactionssDetails, IMapper mapper,
        INumerationService numerationService)
        {
            this.numerationService = numerationService;
            RepTransactionss = repTransactionss;
            RepTransactionssDetails = repTransactionssDetails;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDto data)
        {
            try
            {
                var mapper = _mapper.Map<Transactions>(data);
                string nextNumber = await numerationService.GetNextDocumentAsync(Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb"));
                mapper.Code = nextNumber;
                var result = await RepTransactionss.Insert(mapper);
                var DataSave = await RepTransactionss.SaveChangesAsync();
                await numerationService.SaveNextNumber(Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb"));

                if (DataSave != 1)
                    return Ok(Result<TransactionsDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<TransactionsDto>(result);


                return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (System.Exception ex)
            {
                string mg = ex.Message;
                throw;
            }


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepTransactionss.GetAll();
            var DataSaveDetails = await RepTransactionssDetails.GetAll();
            var DataFillter = DataSave.Where(x => x.IsActive == true).ToList();
            foreach (var item in DataFillter)
            {
                item.TransactionsDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Transactions>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTransactionss.GetById(id);

            
            var DataSaveDetails = await RepTransactionssDetails.GetAll(); 
            
               var transationDetalli = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == id).ToList();
            DataSave.TransactionsDetails = transationDetalli;






            var mapperOut = _mapper.Map<TransactionsDto>(DataSave);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepTransactionss.GetById(id);

            Data.IsActive = false;

            await RepTransactionss.Update(Data);

            var save = await RepTransactionss.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionsDto>(Data);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionsDto _UpdateDto)
        {
            var UpdateData = await RepTransactionss.GetById(_UpdateDto.Id);
            UpdateData.Code = _UpdateDto.Code;
            UpdateData.Reference = _UpdateDto.Reference;
            UpdateData.Commentary = _UpdateDto.Commentary;
            UpdateData.Date = _UpdateDto.Date;
            var result = await RepTransactionss.Update(UpdateData);
            var DataSave = await RepTransactionss.SaveChangesAsync();
            var DataAll = await RepTransactionssDetails.GetAll();
            foreach (var item in DataAll.Where(x => x.TransactionsId == _UpdateDto.Id).ToList())
                item.IsActive = false;


            foreach (var intRow in _UpdateDto.TransactionsDetails)
            {
                if (intRow.Id != null)
                {

                    var rows = await RepTransactionssDetails.GetById(intRow.Id);
                    if (rows != null)
                    {
                        if (intRow.ReferenceId != null)
                            rows.ReferenceId = intRow.ReferenceId;
                        rows.TransactionsId = rows.TransactionsId;
                        rows.Description = intRow.Description;
                        rows.Amount = intRow.Amount;
                        rows.Price = intRow.Price;
                        rows.Discount = intRow.Discount;
                        rows.Total = intRow.Total;
                        rows.Tax = intRow.Tax;
                        rows.Commentary = intRow.Commentary;
                        rows.IsActive = true;
                    }

                }
                else
                {

                    var rows = new TransactionsDetails();
                    if (intRow.ReferenceId != null)
                        rows.ReferenceId = intRow.ReferenceId;
                    rows.TransactionsId = rows.TransactionsId;
                    rows.Description = intRow.Description;
                    rows.Amount = intRow.Amount;
                    rows.Price = intRow.Price;
                    rows.Discount = intRow.Discount;
                    rows.Total = intRow.Total;
                    rows.Tax = intRow.Tax;
                    rows.Commentary = intRow.Commentary;
                    rows.IsActive = true;

                    var insert = await RepTransactionssDetails.Insert(rows);

                }


            }



            var data = await RepTransactionssDetails.SaveChangesAsync();



            return Ok(Result<Transactions>.Success(UpdateData, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
