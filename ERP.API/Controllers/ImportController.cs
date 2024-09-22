using ERP.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<CsvData> CsvData)
        {
            return Ok();
        }
       
    }
}
