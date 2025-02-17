﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class UserAccessDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RolName { get; set; }
        public Guid? RolId { get; set; }
        public string Token { get; set; }

    }

}
