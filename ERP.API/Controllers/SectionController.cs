﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly IGenericRepository<Section> _repSections;

        private readonly IMapper _mapper;
        private int _dataSave;

        public SectionController(IGenericRepository<Section> repSections, IMapper mapper )
        {
            _repSections = repSections;
           
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SectionDto data)
        {
            var mapper = _mapper.Map<Section>(data);


            var result = await _repSections.Insert(mapper);
            try
            {
                _dataSave = await _repSections.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<SectionDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<SectionDto>(result);
                return Ok(Result<SectionDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                
                return Ok(Result<SectionDto>.Fail( ex.Message, MessageCodes.AddedSuccessfully()));
            }

            
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repSections.Find(x => x.IsActive).AsQueryable()
               .ToListAsync();


            var mapperOut = _mapper.Map<SectionDto[]>(dataSave);

            return Ok(Result<SectionDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repSections.GetById(id);

            var mapperOut = _mapper.Map<SectionDto>(dataSave);

            return Ok(Result<SectionDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repSections.GetById(id);

            data.IsActive = false;

            await _repSections.Update(data);

            var save = await _repSections.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<SectionDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<SectionDto>(data);

            return Ok(Result<SectionDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SectionDto updateDto)
        {
            var mapper = _mapper.Map<Section>(updateDto);
            mapper.IsActive = true;
            var result = await _repSections.Update(mapper);

            var dataSave = await _repSections.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<SectionDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<SectionDto>(result);

            return Ok(Result<SectionDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}