using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Services.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ERP.Domain.Entity; 
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
 

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
        private readonly ISysRepository<Formfields> _repFormfields;
        public ReportController(IDirectSql repDynamic, ICurrentUser currentUser, IGenericRepository<PrintingForm> repPrintingForm, 
            IGenericRepository<ReportQuery> repPrintingTemplate, ISysRepository<Formfields> repFormfields, IConfiguration config)
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
            try
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

                        List<ReportParametersDto> _params = new  List<ReportParametersDto>();
                        var Fields = await _repFormfields.Find(x => x.FormId == id && x.IsActive ==true).ToListAsync();
                        if (Fields.Count >0)
                        {
                      
                            foreach (var Field in Fields)
                            {
                                var param = new ReportParametersDto();
                                param.paramName = Field.Field;
                                param.paramValue = data[Field.Field];
                                _params.Add(param);


                            }
                        }

                       

                  

                        conection = _config.GetConnectionString("DefaultConnection");

                        conection = conection.Replace("DbName", currentUser.DataBaseName());

                        var Result = await RepDynamic.QueryDynamic(sqlSelet, _params, conection);
                        return Ok(Result<dynamic>.Success(Result, MessageCodes.AllSuccessfully()));
                    }

                }
                return Ok(Result<PrintingFormDto>.Fail("Error al insentar", MessageCodes.BabData()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(Result<PrintingFormDto>.Fail("Error al insentar", MessageCodes.BabData(),e.Message));
            }

        }

    }
}
