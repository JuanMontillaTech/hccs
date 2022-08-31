using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IDapperServer<Entity>
    {
        public Task<IEnumerable<Entity>> Select(string _filter);
        public Task<int> Insert(string sqlQuery, string _params);
        public Task<IEnumerable<dynamic>> QueryDynamic(string sqlSelet);
    }
}
