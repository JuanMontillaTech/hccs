 
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
    public class BankController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public BankController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Bank")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Banks.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


        

        [Route("/Bank/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Banks.Find(id);
            return Ok(model);
        }

        [Route("/Bank")]
        [HttpPost]
        public IActionResult Post(Bank model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Banks.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Bank")]
        [HttpPut]
        public IActionResult Put(Bank model)
        {
            var Bank = _db.Banks.Find(model.Id);
            Bank.Campo = model.Campo; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Bank")]
        [HttpDelete]
        public IActionResult Delete(Bank model)
        {
            var Bank = _db.Banks.Find(model.Id);
            Bank.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
