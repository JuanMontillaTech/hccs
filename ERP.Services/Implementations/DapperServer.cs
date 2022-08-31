

using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.Contrib;
using System.Threading.Tasks;
using System.Reflection;
using ERP.Domain.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ERP.Services.Implementations
{
    public class DataM
    {
        public string Columname { get; set; }
        public string Value { get; set; }
    }
    public class DapperServer<Entity>
    {
        //public string Connection =
        //     "Server=34.228.63.108;Database=UsuariosProduccion;Trusted_Connection=False;User ID=sa;Password=830434Jr.;MultipleActiveResultSets=True";

        public string Connection =
           "Data Source=CFLYGL3\\SQLEXPRESS;Initial Catalog=ERP_MONTILLA;Integrated Security=True";
        public async Task<IEnumerable<Entity>> Select(string _filter)
        {
            using (var db = new SqlConnection(Connection))
            {

                var sqlSelet = $"select * from  {typeof(Entity).Name} ";
                if (!string.IsNullOrEmpty(_filter))
                {
                    sqlSelet += $" Where {_filter}";
                }

                var List = await db.QueryAsync<Entity>(sqlSelet);

                return List;

            }

        }
        public async Task<IEnumerable<dynamic>> QueryDynamic(string sqlSelet)
        {
            using (var db = new SqlConnection(Connection))
            {
                               
                var List = await db.QueryAsync<dynamic>(sqlSelet);

                return List;

            }

        }


        public async Task<int> Insert(string sqlQuery, string _params)
        {
            using (var db = new SqlConnection(Connection))
            {

                PropertyInfo[] propertyInfos;
                propertyInfos = typeof(Entity).GetProperties(BindingFlags.Public |
                                                              BindingFlags.Static);
                // sort properties by name
                Array.Sort(propertyInfos,
                        delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                        { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

                // write property names
                string Columns = " ";
                int countStart = 0;
                int countEnd = propertyInfos.AsList().Count;
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    countStart++;
                    Columns += propertyInfo.Name;
                    if (countStart < countEnd - 1)
                    {
                        Columns += " , ";
                    }
                }


                var sqlInsert = $"Insert into {typeof(Entity).Name} ({Columns}) Values( {_params})";
                var List = await db.ExecuteAsync(sqlQuery);

                return List;

            }

        }

         
    }
 
}
