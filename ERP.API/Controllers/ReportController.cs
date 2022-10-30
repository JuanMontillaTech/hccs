using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ERP.Domain.Entity;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3.Model;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly IDirectSql RepDynamic;
        private readonly ICurrentUser currentUser;
        private readonly IGenericRepository<PrintingForm> _repPrintingForm;
        private readonly IGenericRepository<ReportQuery> _repPrintingTemplate;
        private readonly IGenericRepository<Formfields> _repFormfields;
        public ReportController(IDirectSql repDynamic, ICurrentUser currentUser, IGenericRepository<PrintingForm> repPrintingForm, 
            IGenericRepository<ReportQuery> repPrintingTemplate, IGenericRepository<Formfields> repFormfields, IConfiguration config)
        {
            RepDynamic = repDynamic;
            this.currentUser = currentUser;
            _repPrintingForm = repPrintingForm;
            _repFormfields =  repFormfields;
            _repPrintingTemplate = repPrintingTemplate;
            _config = config;
        }

        public IConfiguration _config { get; }

 
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id, string Data)
        {
            var PrintForm = await _repPrintingForm.Find(x => x.FormId == id).FirstOrDefaultAsync();
            dynamic data = null;
            if (!string.IsNullOrEmpty(Data))
            {
              data = JsonConvert.DeserializeObject(Data);


            }
            string conection = "";
            if (PrintForm != null)
            {
               var QueryForm = await _repPrintingTemplate.GetById(PrintForm.PrintingTemplateId);
               string sqlSelet = QueryForm.Query;
                if (QueryForm != null)
                {


                    var Fields = await _repFormfields.Find(x => x.FormId == id && x.IsActive ==true).ToListAsync();
                    if (Fields.Count >0)
                    {
                        sqlSelet = sqlSelet + " Where ";
                        foreach (var Field in Fields)
                        {

                        var property_value = Field.Field + $" = '{data[Field.Field]}'" ;
                        sqlSelet += sqlSelet + $" {property_value} ";
                        }
                    }

                       

                  

                    conection = _config.GetConnectionString("DefaultConnection");

                    conection = conection.Replace("DbName", currentUser.DataBaseName());

                    var Result = await RepDynamic.QueryDynamic(sqlSelet, conection);
                    return Ok(Result<dynamic>.Success(Result, MessageCodes.AllSuccessfully()));
                }

            }
            return Ok(Result<PrintingFormDto>.Fail("Error al insentar", MessageCodes.BabData()));

        }

    }
}
