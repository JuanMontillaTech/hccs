using ERP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IDirectSql
    {
        public Task<IEnumerable<dynamic>> QueryDynamic(string sqlSelet, List<ReportParametersDto> _params, string cnn);
    }
}
