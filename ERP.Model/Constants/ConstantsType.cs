using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Constants
{
    public class ConstantsType
    {

        public enum TypeAccountingTransaction
        {
            ExpenseQuates = 0,
            ExpenseCash = 1,
            ExpenseCredit = 2,
            ExpenseReturn = 3,
            InvoiceQuotes = 4,
            InvoiceCredit = 5,
            InvoiceCash = 6,
            InvoceReturn = 7,
            InvoceOrders = 8,
            ExpenseOrders = 9,
            IncomeReceipt = 10,
            ExpenseReceipt = 11
        }
    }
}
