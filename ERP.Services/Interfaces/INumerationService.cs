
using ERP.Domain.Entity;

using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface INumerationService   
    {

        Task<IEnumerable<Numeration>> GetAllByDocumentType();
        Task<string> GetNextDocumentAsync(Guid id);
        Task SaveNextNumber(Guid id);

    }
}
