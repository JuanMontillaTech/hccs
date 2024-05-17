using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IRequestDgiiService
    {
        ResponseQueryRncTaxpayers ConsultRncTaxpayers(string rnc);

        ResponseQueryRncRegistered ConsultRncRegistered(string rnc);

        ResponseQueryNCF ConsultNcf(string ncf, string rnc);
    }
}
