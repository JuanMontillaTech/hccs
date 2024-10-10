using ERP.Domain.Command; 

namespace ERP.Domain.Dtos
{
    public class TransactionLocationDto :AuditDto
    {
    
 
        public string Name { get; set; }

    

        public int Index { get; set; } = 0;
    }
}
