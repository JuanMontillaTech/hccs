//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ERP.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuickLinksController : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok();
//        }
//    }
//}


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
    public class QuickLinksController : ControllerBase
    {
        private readonly IGenericRepository<QuickLinks> _repQuickLinks;

        private readonly IMapper _mapper;

        private int _dataSave;

        public QuickLinksController(IGenericRepository<QuickLinks> repQuickLinks, IMapper mapper)
        {
            _repQuickLinks = repQuickLinks;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] QuickLinksDto data)
        {
            var mapper = _mapper.Map<QuickLinks>(data);
          
           

            var result = await _repQuickLinks.InsertAsync(mapper);

            _dataSave = await _repQuickLinks.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<QuickLinksDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<QuickLinksDto>(result);
            return Ok(Result<QuickLinksDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repQuickLinks.Find(x => x.IsActive).ToListAsync();


            var mapperOut = _mapper.Map<QuickLinksDto[]>(dataSave);

            return Ok(Result<QuickLinksDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<QuickLinksDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var getQuickLinks = _repQuickLinks.Find(x => (x.Title.ToLower().Contains(filter.Search.Trim().ToLower()))
                                           || (x.Type.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();

            int totalRecords = getQuickLinks.Count();
            var dataMaperOut = _mapper.Map<List<QuickLinksDto>>(getQuickLinks);

            var listQuickLinks = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<QuickLinksDto>>.Success(listQuickLinks);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repQuickLinks.GetById(id);

            var mapperOut = _mapper.Map<QuickLinksDto>(dataSave);

            return Ok(Result<QuickLinksDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repQuickLinks.GetById(id);

            data.IsActive = false;

            await _repQuickLinks.Update(data);

            var save = await _repQuickLinks.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<QuickLinksDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<QuickLinksDto>(data);

            return Ok(Result<QuickLinksDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] QuickLinksDto updateDto)
        {
          
            var mapper = _mapper.Map<QuickLinks>(updateDto);

            mapper.IsActive = true;
            var result = await _repQuickLinks.Update(mapper);

            var dataSave = await _repQuickLinks.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<QuickLinksDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<QuickLinksDto>(result);

            return Ok(Result<QuickLinksDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}
