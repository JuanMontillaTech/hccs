using ERP.Domain.Command;
using ERP.Domain.Dtos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface ISecurityService 
    {
        public Task<Result<string>> LoginAsync(string Email, string password);
       
    }
}
