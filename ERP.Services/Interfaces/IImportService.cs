using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IImportService
    {
        public Task<List<RecipePayDto>> ImportRecipeService(List<CsvData> DataForImports);  
    
    }
}
