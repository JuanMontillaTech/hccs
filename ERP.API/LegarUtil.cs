using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API
{
    public class LegarUtil
    {
        public AccountDTtos GetChildrend(AccountDTtos rowsParent, List<LedgerAccount> data)
        {
            var baseChildrens = data;
            var baseUnderChildrens = data;
            foreach (var Row in baseChildrens.Where(x => x.IsActive == true && x.Belongs == rowsParent.Id).ToList())
            {
              AccountDTtos ChildrenNew = new AccountDTtos();
                ChildrenNew.Id = Row.Id;
                ChildrenNew.Text = Row.Name;
                ChildrenNew.Description = Row.Commentary;
                ChildrenNew.AccountCode = Row.Code;
                foreach (var UnderRow in baseUnderChildrens.Where(x => x.IsActive == true && x.Belongs == ChildrenNew.Id).ToList())
                {
                    AccountDTtos ChildrenUnderNew = new AccountDTtos();
                    ChildrenUnderNew.Id = UnderRow.Id;
                    ChildrenUnderNew.Text = UnderRow.Name;
                    ChildrenUnderNew.Description = UnderRow.Commentary;
                    ChildrenUnderNew.AccountCode = UnderRow.Code;
                    var result = GetChildrend(ChildrenUnderNew, baseUnderChildrens);
                    if(result.Id != Guid.Empty)
                    ChildrenUnderNew = result;
                    ChildrenNew.Children.Add(ChildrenUnderNew);
                }

                     return ChildrenNew;
            }
            AccountDTtos childre = new AccountDTtos();
            return childre;
        }
    }
}
