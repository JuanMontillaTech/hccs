using System;

namespace ERP.Domain;


public enum Document
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

public static class TransactionTypes
{
    // Por cobrar
    public static Guid Receivable { get; } = Guid.Parse("85685D53-D6A6-4381-944B-965ED1147FBC");
    // Por pagar
    public static Guid Payable { get; } = Guid.Parse("85685D53-D6A6-4381-944B-965ED1147FBD");
    // Devuelto
    public static Guid Returned { get; } = Guid.Parse("85685D53-D6A6-4381-944B-965ED1187FBA");
    // Pagado
    public static Guid Paid { get; } = Guid.Parse("85685D53-D6A6-4381-944B-965ED1187FBD");
    // Ordenado
    public static Guid Ordered { get; } = Guid.Parse("85685D53-D6A6-4381-944B-995ED1187FBA");
    // Solicitando
    public static Guid Requesting { get; } = Guid.Parse("85685D53-D6A6-4381-944B-995ED1667FBA");
    // Presupuestado
    public static Guid Budgeted { get; } = Guid.Parse("85685D53-D6A6-4381-944B-995ED2167FBA");
    // Despachado
    public static Guid Dispatched { get; } = Guid.Parse("85685D53-D6A6-4381-944B-995ED2667FBA");
    // Cotizado
    public static Guid Quoted { get; } = Guid.Parse("85685D53-D6A6-4381-944B-995ED2967FBA");

    public static Guid GetTransactionType(int TransactionsType)
    {


        switch (TransactionsType)
        {
            case (int)Document.InvoiceCredit: // Por cobrar
                return Receivable;

            case (int)Document.InvoiceCash: // Pagado
                return Paid;

            case (int)Document.InvoceReturn: // Devuelto
                return Returned;

            case (int)Document.ExpenseCash: // Pagado (gasto)
                return Paid;

            case (int)Document.ExpenseCredit: // Por pagar
                return Payable;

            case (int)Document.ExpenseOrders:  // Ordenado (gasto)
                return Guid.Empty;

            case (int)Document.InvoceOrders: // Ordenado (venta)
                return Ordered;

            case (int)Document.InvoiceQuotes: // Cotizado (venta)
                return Quoted;

            case (int)Document.ExpenseQuates: // Presupuestado (gasto) 
                return Budgeted;

            default:
                // Manejo de casos no contemplados (opcional)
                throw new ArgumentException("Tipo de transacción no válido");
        }

    }



}
