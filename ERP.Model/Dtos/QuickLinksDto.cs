using ERP.Domain.Command;
 
namespace ERP.Domain.Dtos
{
    public class QuickLinksDto : AuditDto
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public string Links { get; set; }
     

    }
}
