using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Domain
{
    public interface ICurrentUser
    {
        string UserEmail();

        string DataBaseName();
        
       
        bool UserIsInRole();
        bool IsAnonymous();
    }
}
