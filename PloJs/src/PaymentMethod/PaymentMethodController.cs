 
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
    public class PaymentMethodController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public PaymentMethodController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/PaymentMethod")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.PaymentMethods.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


           [Route("/PaymentMethod/List")]
        [HttpGet]
        public IActionResult List()
        {
            var result = _db.PaymentMethods.Where(x => x.IsActive == true).ToList();
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

        [Route("/PaymentMethod/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.PaymentMethods.Find(id);
            return Ok(model);
        }

        [Route("/PaymentMethod")]
        [HttpPost]
        public IActionResult Post(PaymentMethod model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.PaymentMethods.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/PaymentMethod")]
        [HttpPut]
        public IActionResult Put(PaymentMethod model)
        {
            var PaymentMethod = _db.PaymentMethods.Find(model.Id);
            PaymentMethod.Name = model.Name; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/PaymentMethod")]
        [HttpDelete]
        public IActionResult Delete(PaymentMethod model)
        {
            var PaymentMethod = _db.PaymentMethods.Find(model.Id);
            PaymentMethod.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
