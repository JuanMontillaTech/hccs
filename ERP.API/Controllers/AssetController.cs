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
    public class AssetController : ControllerBase
    {
        private readonly IGenericRepository<Asset> _repository;
        private readonly IMapper _mapper;

        public AssetController(IGenericRepository<Asset> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AssetDto data)
        {
            var entity = _mapper.Map<Asset>(data);
            var result = await _repository.InsertAsync(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetDto>(result);
            return Ok(Result<AssetDto>.Success(dto, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _repository.Find(x => x.IsActive)
                                           .Include(a => a.AssetClass)
                                           .Include(a => a.Location)
                                           .ToListAsync();
            var dtos = _mapper.Map<AssetDto[]>(entities);
            return Ok(Result<AssetDto[]>.Success(dtos, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<AssetDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var entities = _repository.Find(x => x.IsActive &&
                                             (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()) ||
                                              x.SerialNumber.ToLower().Contains(filter.Search.Trim().ToLower()) ||
                                              x.Brand.ToLower().Contains(filter.Search.Trim().ToLower()) ||
                                              x.Model.ToLower().Contains(filter.Search.Trim().ToLower())))
                                      .Include(a => a.AssetClass)
                                      .Include(a => a.Location)
                                      .ToList();

            int totalRecords = entities.Count();
            var dtos = _mapper.Map<List<AssetDto>>(entities);
            var pagedResult = dtos.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<AssetDto>>.Success(pagedResult);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var entity = await _repository.Find(x => x.Id == id)
                                           .Include(a => a.AssetClass)
                                           .Include(a => a.Location)
                                           .FirstOrDefaultAsync();
            var dto = _mapper.Map<AssetDto>(entity);
            return Ok(Result<AssetDto>.Success(dto, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetById(id);
            entity.IsActive = false;
            await _repository.Update(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetDto>(entity);
            return Ok(Result<AssetDto>.Success(dto, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AssetDto updateDto)
        {
            var entity = _mapper.Map<Asset>(updateDto);
            entity.IsActive = true;
            var result = await _repository.Update(entity);
            await _repository.SaveChangesAsync();
            var dto = _mapper.Map<AssetDto>(result);
            return Ok(Result<AssetDto>.Success(dto, MessageCodes.UpdatedSuccessfully()));
        }
    }
}