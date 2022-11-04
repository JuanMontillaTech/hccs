using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class DirectSql : IDirectSql
    {
        public async Task<IEnumerable<dynamic>> QueryDynamic(string sqlSelet, List<ReportParametersDto> _params, string cnn)
        {
            var Dbs =  new DapperServer<dynamic>();
           var Result =  await Dbs.SelectParams(sqlSelet, _params, cnn);
 
            return Result;

            

        }
    }
}
