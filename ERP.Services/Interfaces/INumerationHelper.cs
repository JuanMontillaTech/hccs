using System;
using ERP.Domain.Entity;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces 
{
    public interface INumerationHelper
    {
    
       
        Task<string> GetNextNumerationSequence(Guid formId);
        Task<string> ValidateAndFetchNextTaxNumber(Guid? contactId);
    }
}
