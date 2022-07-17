using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{



    public class MenuDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public bool IsTitle { get; set; }

        public List<SubItem> SubItems { get; set; }
    }
    public class SubItem
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Link { get; set; }        

        public string ParentId { get; set; }
    }
}
