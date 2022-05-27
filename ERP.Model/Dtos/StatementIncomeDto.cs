using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Dtos {

    public class StatementIncomeDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public string Type { get; set; }
        public string Critery { get; set; }
        public List<JournaDetailsDto> Details { get; set; }
    }

    public class StatementIncomeGlobalDto {
        public decimal Amount { get; set; }
        public List<StatementIncomeDto> StatementIncomes { get; set; }
    }
}