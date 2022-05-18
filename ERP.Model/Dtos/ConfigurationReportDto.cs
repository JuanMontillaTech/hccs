using ERP.Domain.Command;

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class ConfigurationReportDto
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public Guid? Parameter { get; set; }
        public string Criterion { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public int Index { get; set; } = 0;

    }
    public class ConfigurationReportIdDto : AuditDto
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public Guid? Parameter { get; set; }
        public string Criterion { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public int Index { get; set; } = 0;

    }
}
