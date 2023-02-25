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
    public class ContactController : ControllerBase
    {
        public readonly IGenericRepository<Contact> RepContacts;

        private readonly IMapper _mapper;
        public ContactController(IGenericRepository<Contact> repContacts, IMapper mapper)
        {
            RepContacts = repContacts;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ContactDto data)
        {
            var mapper = _mapper.Map<Contact>(data);

            var result = await RepContacts.InsertAsync(mapper);

            var DataSave = await RepContacts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ContactDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ContactDto>(result);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           
            var Filter = await RepContacts.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<ContactDto[]>(Filter);

            return Ok(Result<ContactDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        
           
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ContactDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter =   RepContacts.Find(x =>  (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))              
             || (x.Phone1.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Where(x => x.IsActive == true).ToList();

            int totalRecords = RepContacts.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<ContactDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ContactDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepContacts.GetById(id);

            var mapperOut = _mapper.Map<ContactDto>(DataSave);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetContactByPhone")]
        public   IActionResult  GetContactByPhone([FromQuery] string Phone)
        {
            var DataSave =   RepContacts.Find(x=> x.Phone1 == Phone).FirstOrDefault();

            var mapperOut = _mapper.Map<ContactDto>(DataSave);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepContacts.GetById(id);

            Data.IsActive = false;

            await RepContacts.Update(Data);

            var save = await RepContacts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ContactDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ContactDto>(Data);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ContactDto _UpdateDto)
        {
            var mapper = _mapper.Map<Contact>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepContacts.Update(mapper);

            var DataSave = await RepContacts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ContactDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ContactDto>(result);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
