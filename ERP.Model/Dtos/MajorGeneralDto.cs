﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos
{
    public class MajorGeneralTotalsDto
    {
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalCreditor { get; set; }
        public decimal TotalDebtor { get; set; }
    }
    public class MajorGeneralDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Creditor { get; set; }
        public decimal Debtor { get; set; }
        public virtual List<MajorGeneralDetallsDto> MajorGeneralDetalls { get; set; }
        

    }
    public class MajorGeneralDetallsDto
    {
        public Guid? AccountId { get; set; }
        public string Code { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public DateTime Date { get; set; }

    }
    public class AccountsBalanceDto
    {
        public CompanyDto Company { get; set; }
        public string Criterion { get; set; }
        public string Code { get; set; }

        public virtual List<AccountMonthGroupDto> AccountMonthGroup { get; set; }


    }
    public class AccountMonthGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual List<AccountMonthBalanceDto> AccountMonthBalance { get; set; }
    }
    public class AccountMonthBalanceDto
    {
        public Guid Id { get; set; }




        public int MonthNumber { get; set; }
        public string Month { get; set; }
        public virtual MajorGeneralDto MajorGeneralDto { get; set; }
    }

}
