using ERP.Domain.Command;
using ERP.Domain.Constants;
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

    
        [HttpPost("LoadCSV")]
        public async Task<IActionResult> LoadCSV([FromBody] ImportSemestersDto data)
        {

            var datasave = await _importService.ImportRecipeService(data);

            return Ok(Result<List<RecipePayDto>>.Success(datasave, MessageCodes.AddedSuccessfully()));
        }
    }
}
