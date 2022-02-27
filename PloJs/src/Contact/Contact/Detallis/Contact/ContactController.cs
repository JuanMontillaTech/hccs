
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

    public class ContactController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public ContactController(SGAContext Db, IGetCurrentUser _user)
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

            var data = _db.Contacts.Include(b => b.ContactDetails).Where(x => x.IsActive == true && x.EntidadId == IdEntity).ToList();

            return Ok(data);
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
            var codeNEw = _db.Numerations.Where(x => x.Group == 90 && x.EntidadId == IdEntity).ToList().FirstOrDefault();
            if (codeNEw != null)
            {

                codeNEw.Sequence++;
                string _Code = $"{codeNEw.Prefix}{codeNEw.Sequence}";
                model.Code = _Code;
            }
            _db.Contacts.Add(model);

            _db.SaveChanges();



            return Ok(model);
        }

        [Route("/Contact")]
        [HttpPut]
        public IActionResult Put(Contact model)
        {
            var ContactFound = _db.Contacts.Find(model.Id);
            ContactFound.Date = model.Date;
            ContactFound.Reference = model.Reference;
            ContactFound.Commentary = model.Commentary;
            foreach (ContactDetails rows in model.ContactDetails)
            {
                var ContactDetailsFound = _db.ContactDetails.Find(rows.Id);
                ContactDetailsFound.Commentary = rows.Commentary;
                ContactDetailsFound.Debit = rows.Debit;
                ContactDetailsFound.Credit = rows.Credit;
                ContactDetailsFound.Contact = rows.Contact;
            }
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
