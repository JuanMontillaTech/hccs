﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Domain.Entity
{
   
    public class SysUser : Audit
    {
      
        public string Email { get; set; }
        public string Password { get; set; }
        public string DataBaseName { get; set; }
        public bool IsAdmin { get; set; } = false;
        

    }
}
