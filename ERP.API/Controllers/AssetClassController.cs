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
    public class AssetClassController : ControllerBase
    {
        private readonly IGenericRepository<AssetClass> _repository;
        private readonly IMapper _mapper;

        public AssetClassController(IGenericRepository<AssetClass> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AssetClassDto data)
        {
            var entity = _mapper.Map<AssetClass>(data);
            var result = await _repository.InsertAsync(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetClassDto>(result);
            return Ok(Result<AssetClassDto>.Success(dto, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _repository.Find(x => x.IsActive).ToListAsync();
            var dtos = _mapper.Map<AssetClassDto[]>(entities);
            return Ok(Result<AssetClassDto[]>.Success(dtos, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<AssetClassDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var entities = _repository.Find(x => x.IsActive &&
                                             (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()) ||
                                              x.Description.ToLower().Contains(filter.Search.Trim().ToLower())))
                                      .ToList();

            int totalRecords = entities.Count();
            var dtos = _mapper.Map<List<AssetClassDto>>(entities);
            var pagedResult = dtos.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<AssetClassDto>>.Success(pagedResult);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var entity = await _repository.GetById(id);
            var dto = _mapper.Map<AssetClassDto>(entity);
            return Ok(Result<AssetClassDto>.Success(dto, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetById(id);
            entity.IsActive = false;
            await _repository.Update(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetClassDto>(entity);
            return Ok(Result<AssetClassDto>.Success(dto, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AssetClassDto updateDto)
        {
            var entity = _mapper.Map<AssetClass>(updateDto);
            entity.IsActive = true;
            var result = await _repository.Update(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetClassDto>(result);
            return Ok(Result<AssetClassDto>.Success(dto, MessageCodes.UpdatedSuccessfully()));
        }
    }
}