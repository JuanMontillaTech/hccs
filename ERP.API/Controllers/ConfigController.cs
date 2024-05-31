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
    public class ConfigController : ControllerBase
    {
        private readonly IGenericRepository<SysConfig> _repConfig;

        private readonly IMapper _mapper; 
        public ConfigController(IGenericRepository<SysConfig> repConfig, IMapper mapper)
        {
            _repConfig = repConfig;

            _mapper = mapper;
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<SysConfigDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            filter.PageSize = 10;

            var Filter = _repConfig.Find(x => (x.IsActive == true) && (x.ConfigKey == "ST"))
                .ToList();

            int totalRecords = Filter.Count();
            var dataMaperOut = _mapper.Map<List<SysConfigDto>>(Filter);

            var list = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<SysConfigDto>>.Success(list);
            return Ok(result);

        }

    }
}
