using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class DirectSql : IDirectSql
    {
        public Task<IEnumerable<dynamic>> QueryDynamic(string sqlSelet, string cnn)
        {
            var Dbs =  new DapperServer<dynamic>();
           var Result = Dbs.QueryDynamic(sqlSelet, cnn);

            return Result;



        }
    }
}
