 
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
    public class NumerationController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public NumerationController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Numeration")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Numerations.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


        

        [Route("/Numeration/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Numerations.Find(id);
            return Ok(model);
        }

        [Route("/Numeration")]
        [HttpPost]
        public IActionResult Post(Numeration model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Numerations.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Numeration")]
        [HttpPut]
        public IActionResult Put(Numeration model)
        {
            var Numeration = _db.Numerations.Find(model.Id);
            Numeration.Campo = model.Campo; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Numeration")]
        [HttpDelete]
        public IActionResult Delete(Numeration model)
        {
            var Numeration = _db.Numerations.Find(model.Id);
            Numeration.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
