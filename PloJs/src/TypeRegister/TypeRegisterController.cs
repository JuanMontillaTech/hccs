 
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
    public class TypeRegisterController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public TypeRegisterController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/TypeRegister")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.TypeRegisters.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


        

        [Route("/TypeRegister/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.TypeRegisters.Find(id);
            return Ok(model);
        }

        [Route("/TypeRegister")]
        [HttpPost]
        public IActionResult Post(TypeRegister model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.TypeRegisters.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/TypeRegister")]
        [HttpPut]
        public IActionResult Put(TypeRegister model)
        {
            var TypeRegister = _db.TypeRegisters.Find(model.Id);
            TypeRegister.Campo = model.Campo; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/TypeRegister")]
        [HttpDelete]
        public IActionResult Delete(TypeRegister model)
        {
            var TypeRegister = _db.TypeRegisters.Find(model.Id);
            TypeRegister.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
