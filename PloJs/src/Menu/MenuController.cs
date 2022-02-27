 
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
    public class MenuController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public MenuController(SGAContext Db , IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;
        }

     
        public IActionResult Index()
        {
            
            return View(); 
        }
        [Route("/Menu")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var model = _db.Menus.Where(x=> x.IsActive == true && x.EntidadId == IdEntity).ToList();
            return Ok(model);
        }


           [Route("/Menu/List")]
        [HttpGet]
        public IActionResult List()
        {
            var result = _db.Menus.Where(x => x.IsActive == true).ToList();
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

        [Route("/Menu/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Menus.Find(id);
            return Ok(model);
        }

        [Route("/Menu")]
        [HttpPost]
        public IActionResult Post(Menu model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
          
                _db.Menus.Add(model);
                _db.SaveChanges();
       

            return Ok(model);
        }

        [Route("/Menu")]
        [HttpPut]
        public IActionResult Put(Menu model)
        {
            var Menu = _db.Menus.Find(model.Id);
            Menu.Name = model.Name; 

            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Menu")]
        [HttpDelete]
        public IActionResult Delete(Menu model)
        {
            var Menu = _db.Menus.Find(model.Id);
            Menu.IsActive = false; 
            _db.SaveChanges();

            return Ok(model);
        }
    }

}
