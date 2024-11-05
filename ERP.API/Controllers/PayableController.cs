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
    public class PayableController : ControllerBase
    {
        private readonly IGenericRepository<Payable> _repository;
        private readonly IMapper _mapper;

        public PayableController(IGenericRepository<Payable> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PayableDto data)
        {
            try
            {
                var payable = _mapper.Map<Payable>(data);

                // Aquí debes obtener el usuario actual para las propiedades de auditoría
                // payable.CreatedBy = currentUserId; 
                // payable.LastModifiedBy = currentUserId;
                payable.CreatedDate = DateTime.UtcNow;
                payable.LastModifiedDate = DateTime.UtcNow;
                payable.IsActive = true;

                var result = await _repository.InsertAsync(payable);
                await _repository.SaveChangesAsync();

                var mapperOut = _mapper.Map<PayableDto>(result);
                return Ok(Result<PayableDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                // Registrar la excepción (e.g., usando un logger)
                return Ok(Result<PayableDto>.Fail(MessageCodes.ErrorCreating, ex.Message));
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var payables = await _repository.Find(x => x.IsActive)
                .Include(x => x.Contact)
             .Include(x => x.PaymentMethod)
                .ToListAsync();

            var mapperOut = _mapper.Map<PayableDto[]>(payables);
            return Ok(Result<PayableDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<PayableDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var payables = _repository.Find(x => x.IsActive && 
                                                (x.DocumentNumber.ToLower().Contains(filter.Search.Trim().ToLower()) ||
                                                 x.Description.ToLower().Contains(filter.Search.Trim().ToLower())))
                                       .ToList();

            int totalRecords = payables.Count();
            var dataMaperOut = _mapper.Map<List<PayableDto>>(payables);

            var listPayables = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<PayableDto>>.Success(listPayables);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var payable = await _repository.GetById(id);

            if (payable == null)
            {
                return NotFound(Result<PayableDto>.Fail( MessageCodes.NotFound(), "77"));
            }

            var mapperOut = _mapper.Map<PayableDto>(payable);
            return Ok(Result<PayableDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var payable = await _repository.GetById(id);

                if (payable == null)
                {
                    return NotFound(Result<PayableDto>.Fail( MessageCodes.ErrorDeleting, "99"));
                }

                payable.IsActive = false;
                // payable.LastModifiedBy = currentUserId;
                payable.LastModifiedDate = DateTime.UtcNow;

                await _repository.Update(payable);
                await _repository.SaveChangesAsync();

                var mapperOut = _mapper.Map<PayableDto>(payable);
                return Ok(Result<PayableDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
            }
            catch (Exception ex)
            {
                // Registrar la excepción 
                return Ok(Result<PayableDto>.Fail(MessageCodes.ErrorDeleting, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PayableDto updateDto)
        {
            try
            {
                var payable = await _repository.GetById(updateDto.Id);

                if (payable == null)
                {
                    return NotFound(Result<PayableDto>.Fail( MessageCodes.NotFound(), "88"));
                }

                _mapper.Map(updateDto, payable);
                // payable.LastModifiedBy = currentUserId;
                payable.LastModifiedDate = DateTime.UtcNow;

                await _repository.Update(payable);
                await _repository.SaveChangesAsync();

                var mapperOut = _mapper.Map<PayableDto>(payable);
                return Ok(Result<PayableDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
            }
            catch (Exception ex)
            {
                // Registrar la excepción 
                return Ok(Result<PayableDto>.Fail(MessageCodes.ErrorUpdating, ex.Message));
            }
        }
    }
}