using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using System.Net;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        public readonly IGenericRepository<PaymentMethod> RepPaymentMethods;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentMethodController(IGenericRepository<PaymentMethod> repPaymentMethods, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepPaymentMethods = repPaymentMethods;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDto data)
        {
            var mapper = _mapper.Map<PaymentMethod>(data);

            var result = await RepPaymentMethods.Insert(mapper);

            var DataSave = await RepPaymentMethods.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PaymentMethodDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<PaymentMethodDto>(result);

            return Ok(Result<PaymentMethodDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepPaymentMethods.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Banks).ToListAsync();

          
            var mapperOut = _mapper.Map<PaymentMethodDto[]>(DataSave);

            return Ok(Result<PaymentMethodDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }


        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<PaymentMethodDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepPaymentMethods.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             

            ).ToList();

            int totalRecords = RepPaymentMethods.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<PaymentMethodDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<PaymentMethodDto>>.Success(List);
            return Ok(Result);

        }


        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepPaymentMethods.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<PaymentMethodDto>(Filter);

            return Ok(Result<PaymentMethodDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepPaymentMethods.GetById(id);

            var mapperOut = _mapper.Map<PaymentMethodDto>(DataSave);

            return Ok(Result<PaymentMethodDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepPaymentMethods.GetById(id);

            Data.IsActive = false;

            await RepPaymentMethods.Update(Data);

            var save = await RepPaymentMethods.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<PaymentMethodDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<PaymentMethodDto>(Data);

            return Ok(Result<PaymentMethodDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PaymentMethodDto _UpdateDto)
        {

            var mapper = _mapper.Map<PaymentMethod>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepPaymentMethods.Update(mapper);

            var DataSave = await RepPaymentMethods.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PaymentMethodDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<PaymentMethodDto>(result);

            return Ok(Result<PaymentMethodDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
