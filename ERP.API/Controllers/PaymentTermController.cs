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
    public class PaymentTermController : ControllerBase
    {
        public readonly IGenericRepository<PaymentTerm> RepPaymentTerms;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentTermController(IGenericRepository<PaymentTerm> repPaymentTerms, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepPaymentTerms = repPaymentTerms;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentTermDto data)
        {
            var mapper = _mapper.Map<PaymentTerm>(data);

            var result = await RepPaymentTerms.InsertAsync(mapper);

            var DataSave = await RepPaymentTerms.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PaymentTermDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<PaymentTermDto>(result);

            return Ok(Result<PaymentTermDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            var Filter = await RepPaymentTerms.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<PaymentTermDto[]>(Filter);

            return Ok(Result<PaymentTermDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<PaymentTermDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepPaymentTerms.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<PaymentTermDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<PaymentTermDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepPaymentTerms.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<PaymentTermDto>(Filter);

            return Ok(Result<PaymentTermDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepPaymentTerms.GetById(id);

            var mapperOut = _mapper.Map<PaymentTermDto>(DataSave);

            return Ok(Result<PaymentTermDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepPaymentTerms.GetById(id);

            Data.IsActive = false;

            await RepPaymentTerms.Update(Data);

            var save = await RepPaymentTerms.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<PaymentTermDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<PaymentTermDto>(Data);

            return Ok(Result<PaymentTermDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PaymentTermDto _UpdateDto)
        {

            var mapper = _mapper.Map<PaymentTerm>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepPaymentTerms.Update(mapper);

            var DataSave = await RepPaymentTerms.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PaymentTermDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<PaymentTermDto>(result);

            return Ok(Result<PaymentTermDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
