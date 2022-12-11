using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{

    public class PosDto
    {
       public Guid FormId { get; set; }
        public Guid TransactionLocationId { get; set; }
        public List<PosDtoItem> TransactionsItems { get; set; }
        public int TransactionType { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }

      


    }

    public class PosDtoItem
    {
        public Guid Id { get; set; }
        public decimal Count { get; set; }
        public string Description { get; set; }
        public decimal PriceSale { get; set; }
        public List<ConceptElementDto> ElementConcept { get; set; }
    }

}
