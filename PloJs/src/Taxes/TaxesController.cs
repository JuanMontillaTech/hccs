 
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
    public class TaxesController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public TaxesController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Taxes")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Taxess.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


           [Route("/Taxes/List")]
        [HttpGet]
        public IActionResult List()
        {
            var result = _db.Taxess.Where(x => x.IsActive == true).ToList();
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

        [Route("/Taxes/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Taxess.Find(id);
            return Ok(model);
        }

        [Route("/Taxes")]
        [HttpPost]
        public IActionResult Post(Taxes model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Taxess.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Taxes")]
        [HttpPut]
        public IActionResult Put(Taxes model)
        {
            var Taxes = _db.Taxess.Find(model.Id);
            Taxes.Name = model.Name; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Taxes")]
        [HttpDelete]
        public IActionResult Delete(Taxes model)
        {
            var Taxes = _db.Taxess.Find(model.Id);
            Taxes.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
