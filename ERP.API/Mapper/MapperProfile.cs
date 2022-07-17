using AutoMapper;

using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;

namespace ERP.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ContactDto, Contact>().ReverseMap();
            CreateMap<ContactIdDto, Contact>().ReverseMap();
            CreateMap<ConceptDto, Concept>().ReverseMap();
            CreateMap<ConceptIdDto, Concept>().ReverseMap();
            CreateMap<LedgerAccountDto, LedgerAccount>().ReverseMap();
            CreateMap<LedgerAccountIdDto, LedgerAccount>().ReverseMap();
            CreateMap<JournalDto, Journal>().ReverseMap();
            CreateMap<JournaDetails, JournaDetailsDto>().ReverseMap();
            CreateMap<Transactions, TransactionsDto>().ReverseMap();
            CreateMap<TransactionsDetails, TransactionsDetailsDto>().ReverseMap();
            CreateMap<TransactionStatus, TransactionStatusDto>().ReverseMap();
            CreateMap<Numeration, NumerationDto>().ReverseMap(); 
            CreateMap<TransactionReceipt, TransactionReceiptDto>().ReverseMap();
            CreateMap<ConfigurationPurchase, ConfigurationPurchaseDto>().ReverseMap();
            CreateMap<Taxes, TaxesDto>().ReverseMap();
            CreateMap<Taxes, TaxesIdDto>().ReverseMap();
            CreateMap<Form, FormDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();

            CreateMap<ConfigurationReport, ConfigurationReportIdDto>().ReverseMap();
            CreateMap<ConfigurationReport, ConfigurationReportDto>().ReverseMap();
          
            CreateMap<Payroll, PayrollDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            
            CreateMap<Company, CompanyDto>().ReverseMap();

            CreateMap<ConfigurationSell, ConfigurationSellDto>().ReverseMap();
            CreateMap<Company, CompanyIdDto>().ReverseMap();


        }
    }
}
