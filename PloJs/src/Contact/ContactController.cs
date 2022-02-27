 
using ERP.Model.Command;
using ERP.Model.Dtos;
using ERP.Model.Entity;
using ERP.Services.Constants;
using ERP.Services.Implementations;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGA.Mappings;
using System.ComponentModel;
using System.Reflection;
using ERP.Model;

namespace SGA.Controllers
{
    public class ContactController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public ContactController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Contact")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Contacts.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


           [Route("/Contact/List")]
        [HttpGet]
        public IActionResult List()
        {
            var result = _db.Contacts.Where(x => x.IsActive == true).ToList();
            List<GenericListDto> DataList = new List<GenericListDto>();
            foreach (var row in result)
            {
                DataList.Add(new GenericListDto()
                {
                    Id = row.Id.ToString(),
                    Name = $"{row.Name}"

                });
            }
            return new JsonResult(Result<List<GenericListDto>>.Success(DataList, ""));
        }

        [Route("/Contact/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Contacts.Find(id);
            return Ok(model);
        }

        [Route("/Contact")]
        [HttpPost]
        public IActionResult Post(Contact model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Contacts.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Contact")]
        [HttpPut]
        public IActionResult Put(Contact model)
        {
            var Contact = _db.Contacts.Find(model.Id);
            Contact.Name = model.Name; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Contact")]
        [HttpDelete]
        public IActionResult Delete(Contact model)
        {
            var Contact = _db.Contacts.Find(model.Id);
            Contact.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
