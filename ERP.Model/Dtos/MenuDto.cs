using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{

    public class UserDto
    {
        public string UserName { get; set; }
        public List<MenuDto> Menu { get; set; }

    }


    public class MenuDto
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<MenuOptionDto> Option { get; set; }
    }
    public class MenuOptionDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}
