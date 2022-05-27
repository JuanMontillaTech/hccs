//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ERP.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CompanyController : ControllerBase
//    {
//    }
//}
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

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class CompanyController : ControllerBase
    {
        public readonly IGenericRepository<Company> RepCompanys;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyController(IGenericRepository<Company> repCompanys, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepCompanys = repCompanys;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CompanyDto data)
        {
            var mapper = _mapper.Map<Company>(data);

            var result = await RepCompanys.Insert(mapper);

            var DataSave = await RepCompanys.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<CompanyIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<CompanyIdDto>(result);

            return Ok(Result<CompanyIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepCompanys.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<CompanyIdDto[]>(Filter);

            return Ok(Result<CompanyIdDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepCompanys.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<CompanyIdDto>(Filter);

            return Ok(Result<CompanyIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepCompanys.GetById(id);

            var mapperOut = _mapper.Map<CompanyIdDto>(DataSave);

            return Ok(Result<CompanyIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepCompanys.GetById(id);

            Data.IsActive = false;

            await RepCompanys.Update(Data);

            var save = await RepCompanys.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<CompanyIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<CompanyIdDto>(Data);

            return Ok(Result<CompanyIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CompanyIdDto _UpdateDto)
        {

            var mapper = _mapper.Map<Company>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepCompanys.Update(mapper);

            var DataSave = await RepCompanys.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<CompanyIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<CompanyIdDto>(result);

            return Ok(Result<CompanyIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
