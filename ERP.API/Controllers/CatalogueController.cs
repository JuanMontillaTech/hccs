
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
    public class CatalogueController : ControllerBase
    {
        private readonly IGenericRepository<Catalogue> _repCatalogue;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CatalogueController(IGenericRepository<Catalogue> repCatalogue, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repCatalogue = repCatalogue;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CatalogueDto data)
        {
            var mapper = _mapper.Map<Catalogue>(data);


            var result = await _repCatalogue.Insert(mapper);
            try
            {
                var DataSave = await _repCatalogue.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<CatalogueDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<CatalogueDto>(result);
                return Ok(Result<CatalogueDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<CatalogueDto>.Fail("Error al insertar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repCatalogue.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.AccountCommissionList)
                .Include(x=> x.AccountCostList)
                .Include(x=> x.AccountSalesLis)
                .Include(x=> x.AccountDiscountList)
                .Include(x=> x.AccountReturnedList)
                .Include(x=> x.AccountCommissionList) .ToListAsync();
             

            var mapperOut = _mapper.Map<CatalogueDto[]>(dataSave);

            return Ok(Result<CatalogueDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<CatalogueDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repCatalogue.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            
            ).ToList();

            int totalRecords = _repCatalogue.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<CatalogueDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<CatalogueDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repCatalogue.GetById(id);

            var mapperOut = _mapper.Map<CatalogueDto>(dataSave);

            return Ok(Result<CatalogueDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repCatalogue.GetById(id);

            data.IsActive = false;

            await _repCatalogue.Update(data);

            var save = await _repCatalogue.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<CatalogueDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<CatalogueDto>(data);

            return Ok(Result<CatalogueDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CatalogueDto updateDto)
        {

            var mapper = _mapper.Map<Catalogue>(updateDto);
            mapper.IsActive = true;
            var result = await _repCatalogue.Update(mapper);

            var dataSave = await _repCatalogue.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<CatalogueDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<CatalogueDto>(result);

            return Ok(Result<CatalogueDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
