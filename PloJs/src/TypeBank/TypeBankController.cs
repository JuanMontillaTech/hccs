 
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
    public class TypeBankController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public TypeBankController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/TypeBank")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.TypeBanks.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


        

        [Route("/TypeBank/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.TypeBanks.Find(id);
            return Ok(model);
        }

        [Route("/TypeBank")]
        [HttpPost]
        public IActionResult Post(TypeBank model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.TypeBanks.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/TypeBank")]
        [HttpPut]
        public IActionResult Put(TypeBank model)
        {
            var TypeBank = _db.TypeBanks.Find(model.Id);
            TypeBank.Campo = model.Campo; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/TypeBank")]
        [HttpDelete]
        public IActionResult Delete(TypeBank model)
        {
            var TypeBank = _db.TypeBanks.Find(model.Id);
            TypeBank.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
