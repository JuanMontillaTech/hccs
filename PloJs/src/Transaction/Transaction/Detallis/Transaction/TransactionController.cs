
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

    public class TransactionsController : Controller
    {

        private readonly SGAContext _db;
        private readonly IGetCurrentUser _getCurrentUser;
        public TransactionsController(SGAContext Db, IGetCurrentUser _user)
        {
            _db = Db;
            _getCurrentUser = _user;

        }


        public IActionResult Index()
        {

            return View();
        }
        [Route("/Transactions")]
        [HttpGet]
        public IActionResult Get()
        {
            int IdEntity = _getCurrentUser.UserEntidadID();

            var data = _db.Transactions.Include(b => b.TransactionDetails).Where(x => x.IsActive == true && x.EntidadId == IdEntity).ToList();

            return Ok(data);
        }




        [Route("/Transactions/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var model = _db.Transactions.Find(id);
            return Ok(model);
        }


        [Route("/Transactions")]
        [HttpPost]
        public IActionResult Post(Transaction model)
        {
            int IdEntity = _getCurrentUser.UserEntidadID();
            var codeNEw = _db.Numerations.Where(x => x.Group == 90 && x.EntidadId == IdEntity).ToList().FirstOrDefault();
            if (codeNEw != null)
            {

                codeNEw.Sequence++;
                string _Code = $"{codeNEw.Prefix}{codeNEw.Sequence}";
                model.Code = _Code;
            }
            _db.Transactions.Add(model);

            _db.SaveChanges();



            return Ok(model);
        }

        [Route("/Transactions")]
        [HttpPut]
        public IActionResult Put(Transaction model)
        {
            var TransactionFound = _db.Transactions.Find(model.Id);
            TransactionFound.Date = model.Date;
            TransactionFound.Reference = model.Reference;
            TransactionFound.Commentary = model.Commentary;
            foreach (TransactionDetails rows in model.TransactionDetails)
            {
                var TransactionDetailsFound = _db.TransactionDetails.Find(rows.Id);
                TransactionDetailsFound.Commentary = rows.Commentary;
                TransactionDetailsFound.Debit = rows.Debit;
                TransactionDetailsFound.Credit = rows.Credit;
                TransactionDetailsFound.Contact = rows.Contact;
            }
            _db.SaveChanges();

            return Ok(model);
        }

        [Route("/Transactions")]
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
