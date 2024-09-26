using ERP.Domain.Dtos;
using ERP.Services.Interfaces;
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
        IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<CsvData> CsvData)
        {
            await _importService.ImportRecipeService(CsvData);
            return Ok();
        }
       
    }
}
