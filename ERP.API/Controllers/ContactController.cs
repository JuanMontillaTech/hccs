using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
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

            var result = await RepContacts.Insert(mapper);

            var DataSave = await RepContacts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ContactIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ContactIdDto>(result);

            return Ok(Result<ContactIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
 var DataSave = await RepContacts.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<ContactIdDto[]>(Filter);

            return Ok(Result<ContactIdDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepContacts.GetById(id);

            var mapperOut = _mapper.Map<ContactIdDto>(DataSave);

            return Ok(Result<ContactIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetContactByPhone")]
        public   IActionResult  GetContactByPhone([FromQuery] string Phone)
        {
            var DataSave =   RepContacts.Find(x=> x.Phone1 == Phone).FirstOrDefault();

            var mapperOut = _mapper.Map<ContactIdDto>(DataSave);

            return Ok(Result<ContactIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepContacts.GetById(id);

            Data.IsActive = false;

            await RepContacts.Update(Data);

            var save = await RepContacts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ContactIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ContactIdDto>(Data);

            return Ok(Result<ContactIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ContactIdDto _UpdateDto)
        {
            var mapper = _mapper.Map<Contact>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepContacts.Update(mapper);

            var DataSave = await RepContacts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ContactIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ContactIdDto>(result);

            return Ok(Result<ContactIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
