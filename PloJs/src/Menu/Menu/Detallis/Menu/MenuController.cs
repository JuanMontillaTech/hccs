
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
using Microsoft.EntityFrameworkCore;

namespace SGA.Controllers
{

    public class MenuController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public MenuController(SGAContext Db, IGetCurrentUser _user)
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

            var data = _db.Menus.Include(b => b.MenuDetails).Where(x => x.IsActive == true && x.EntidadId == IdEntity).ToList();

            return Ok(data);
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
            var codeNEw = _db.Numerations.Where(x => x.Group == 90 && x.EntidadId == IdEntity).ToList().FirstOrDefault();
            if (codeNEw != null)
            {

                codeNEw.Sequence++;
                string _Code = $"{codeNEw.Prefix}{codeNEw.Sequence}";
                model.Code = _Code;
            }
            _db.Menus.Add(model);

            _db.SaveChanges();



            return Ok(model);
        }

        [Route("/Menu")]
        [HttpPut]
        public IActionResult Put(Menu model)
        {
            var MenuFound = _db.Menus.Find(model.Id);
            MenuFound.Date = model.Date;
            MenuFound.Reference = model.Reference;
            MenuFound.Commentary = model.Commentary;
            foreach (MenuDetails rows in model.MenuDetails)
            {
                var MenuDetailsFound = _db.MenuDetails.Find(rows.Id);
                MenuDetailsFound.Commentary = rows.Commentary;
                MenuDetailsFound.Debit = rows.Debit;
                MenuDetailsFound.Credit = rows.Credit;
                MenuDetailsFound.Contact = rows.Contact;
            }
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
