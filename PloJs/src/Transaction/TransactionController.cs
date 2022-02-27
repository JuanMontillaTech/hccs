 
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
    public class TransactionController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public TransactionController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Transaction")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Transactions.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


           [Route("/Transaction/List")]
        [HttpGet]
        public IActionResult List()
        {
            var result = _db.Transactions.Where(x => x.IsActive == true).ToList();
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

        [Route("/Transaction/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Transactions.Find(id);
            return Ok(model);
        }

        [Route("/Transaction")]
        [HttpPost]
        public IActionResult Post(Transaction model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Transactions.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Transaction")]
        [HttpPut]
        public IActionResult Put(Transaction model)
        {
            var Transaction = _db.Transactions.Find(model.Id);
            Transaction.Name = model.Name; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Transaction")]
        [HttpDelete]
        public IActionResult Delete(Transaction model)
        {
            var Transaction = _db.Transactions.Find(model.Id);
            Transaction.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
