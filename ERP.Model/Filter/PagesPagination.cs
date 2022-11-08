using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Filter
{
    public class PagesPagination<T>
    {
        public int TotalRecords { get; set; }

        public int TotalFiltered { get; set; }

        public List<T> Data { get; set; } = new List<T>();

        public int TotalPages { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

    }
}
