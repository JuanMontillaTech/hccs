using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public class ImportSemestersDto
    {
        public HeadSemestersDto headSemesters { get; set; }
        public List<CsvData> data { get; set; }
    }
}
