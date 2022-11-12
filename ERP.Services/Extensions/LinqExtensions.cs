using ERP.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
namespace ERP.Services.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> query, PaginationFilter paginationFilter)
        {
            if (!paginationFilter.ColumnOrdeBy.Trim().Equals("function"))
            {
                if (paginationFilter.IsOrderByDescending && !string.IsNullOrEmpty(paginationFilter.ColumnOrdeBy))
                {
                    query = query.OrderBy($"{paginationFilter.ColumnOrdeBy} descending");
                }
                else if (!string.IsNullOrEmpty(paginationFilter.ColumnOrdeBy) && !paginationFilter.IsOrderByDescending)
                {
                    query = query.OrderBy($"{paginationFilter.ColumnOrdeBy} ascending");
                }
            }

            var queryable = query.Skip(paginationFilter.PageNumber == 1 ? 0 : paginationFilter.PageNumber).Take(paginationFilter.PageSize);

            return queryable;
        }

        public static PagesPagination<T> PaginationPages<T>(this IQueryable<T> query, PaginationFilter paginationFilter, int totalRecords )
        {
            if (query != null && query.Count() > 0)
            {
                if (!paginationFilter.ColumnOrdeBy.Trim().Equals("function"))
                {
                    if (paginationFilter.IsOrderByDescending && !string.IsNullOrEmpty(paginationFilter.ColumnOrdeBy))
                    {
                        query = query.OrderBy($"{paginationFilter.ColumnOrdeBy} descending");
                    }
                    else if (!string.IsNullOrEmpty(paginationFilter.ColumnOrdeBy) && !paginationFilter.IsOrderByDescending)
                    {
                        query = query.OrderBy($"{paginationFilter.ColumnOrdeBy} ascending");
                    }
                }

                var queryable = query.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize).Take(paginationFilter.PageSize);

                int TotalRecordsEntity = queryable.Count();

                var obj = queryable.ToList();

                var pagesPagination = new PagesPagination<T>
                {
                    Data = obj,
                   
                    TotalPages = paginationFilter.PageSize != 0 ? Convert.ToInt32(Math.Ceiling((double)(TotalRecordsEntity / paginationFilter.PageSize) )) :0,

                    PageSize = paginationFilter.PageSize,

                    TotalFiltered = obj.Count(),

                    TotalRecords = TotalRecordsEntity,

                    PageNumber = paginationFilter.PageNumber
                };

                return pagesPagination;
            }
            else
                return new PagesPagination<T>();
        }

    }
}
