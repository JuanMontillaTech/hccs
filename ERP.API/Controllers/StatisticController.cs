﻿using System;
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
    public class StatisticController : ControllerBase
    {
        private readonly IGenericRepository<SysStatistic> _repSysStatistic;

        private readonly IMapper _mapper;

        private int _dataSave;

        public StatisticController(IGenericRepository<SysStatistic> repSysStatistic, IMapper mapper)
        {
            _repSysStatistic = repSysStatistic;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SysStatisticDto data)
        {

            var mapper = _mapper.Map<SysStatistic>(data);


            var result = await _repSysStatistic.InsertAsync(mapper);

            _dataSave = await _repSysStatistic.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<SysStatisticDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<SysStatisticDto>(result);
            return Ok(Result<SysStatisticDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dataSave = await _repSysStatistic.Find(x => x.IsActive).AsQueryable()
        .Include(s => s.Form).Include(x => x.ReportQuery).Include(x => x.Config).ToListAsync();


                var mapperOut = _mapper.Map<SysStatisticDto[]>(dataSave);

                return Ok(Result<SysStatisticDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<SysStatisticDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getSysStatistic = _repSysStatistic.Find(x => x.IsActive == true
            && ((x.Form.Label.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Form.Title.ToLower().Contains(filter.Search.Trim().ToLower())))
            ).Include(x=> x.Form).Include(x=> x.Config).ToList();

            int totalRecords = getSysStatistic.Count();
            var dataMaperOut = _mapper.Map<List<SysStatisticDto>>(getSysStatistic);

            var listSysStatistic = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<SysStatisticDto>>.Success(listSysStatistic);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repSysStatistic.GetById(id);

            var mapperOut = _mapper.Map<SysStatisticDto>(dataSave);

            return Ok(Result<SysStatisticDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repSysStatistic.GetById(id);

            data.IsActive = false;

            await _repSysStatistic.Update(data);

            var save = await _repSysStatistic.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<SysStatisticDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<SysStatisticDto>(data);

            return Ok(Result<SysStatisticDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        /// <summary>
        /// Get role information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetStatisticByFormId")]
        [ProducesResponseType(typeof(Result<ICollection<SysStatisticDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetStatisticByFormId([FromQuery] Guid id)
        {
            var Filter = _repSysStatistic.Find(x => x.IsActive == true && x.FormId == id).Include(s => s.Form).Include(x => x.ReportQuery).ToList();
            var dataMaperOut = _mapper.Map<List<SysStatisticDto>>(Filter);
            var result = Result<List<SysStatisticDto>>.Success(dataMaperOut);
            return Ok(result);
        }



        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SysStatisticDto updateDto)
        {
            var mapper = _mapper.Map<SysStatistic>(updateDto);
            mapper.IsActive = true;
            var result = await _repSysStatistic.Update(mapper);

            var dataSave = await _repSysStatistic.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<SysStatisticDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<SysStatisticDto>(result);

            return Ok(Result<SysStatisticDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}
