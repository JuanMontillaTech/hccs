using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface ICurrentUser
    {
        string UserEmail();
        Guid UserId();
        Guid Roll();

        string DataBaseName();
        string Sub();
        
       
        bool UserIsInRole();
        bool IsAnonymous();
    }
}
