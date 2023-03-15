using System.Collections.Generic;
using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class RecipeDto  : AuditDto
{

    public TransactionReceiptDto TransactionReceipt { get; set; }

    public PaymentMethodDto PaymentMethod { get; set; }
    
    public BankDto Bank { get; set; }
    
    public  CurrencyDto Currency  { get; set; }
    
    public ContactDto  Contact  { get; set; }
    
    public List<TransactionReceiptDetailsDto> TransactionReceiptDetails  { get; set; }
    
    
    
}