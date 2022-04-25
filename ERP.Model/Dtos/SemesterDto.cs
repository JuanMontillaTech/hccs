using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class SemesterDto
    {
        public CompanyDto Company { get; set; }
        public virtual List<SemesterDetailsDto> Icome  { get; set; }
        public virtual List<SemesterDetailsDto> Expense { get; set; }
        public virtual List<SemesterDetailsDto> Bank { get; set; }
        public virtual List<SemesterDetailsDto> AvailableAssets { get; set; }
        public virtual List<SemesterDetailsDto> Outcome { get; set; }       


    }
    public class SemesterDetailsDto
    {
        public string Month { get; set; }
        public virtual List<MajorGeneralDto> Account { get; set; }
        public decimal Total { get; set; }

    }

}
