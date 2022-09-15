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
            CreateMap<PrintingTemplateDto, PrintingTemplate>().ReverseMap();
            CreateMap<SectionDto, Section>().ReverseMap();
            CreateMap<SectionFieldsDto, Section>().ReverseMap();
            CreateMap<SectionFormGrid, Section>().ReverseMap();
            CreateMap<CatalogueDto, Catalogue>().ReverseMap();
            CreateMap<PaymentTermDto, PaymentTerm>().ReverseMap(); 
            CreateMap<Banks, BankDto>().ReverseMap();
            CreateMap<Banks, BankDetallisDto>().ReverseMap(); 
            CreateMap<Form, FormDto>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
            CreateMap<Roll, RollDto>().ReverseMap();
            CreateMap<RollForm, RollFormDto>().ReverseMap();
            CreateMap<RollForm, RollFormDetallisDto>().ReverseMap();
            CreateMap<Formfields, FormfieldsDto>().ReverseMap();
            CreateMap<Formfields, FormfieldsDetallisDto>().ReverseMap();             
            CreateMap<FormGrid, FormGridDto>().ReverseMap();
            CreateMap<FormGrid, FormGridDtoDetallisDto>().ReverseMap();
            CreateMap<Sys_User, Sys_UserDto>().ReverseMap();
            CreateMap<UserRoll, UserRollDto>().ReverseMap();
            CreateMap<UserRoll, UserRollDetallisDto>().ReverseMap(); 
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
            CreateMap<Currency, CurrencyDto>().ReverseMap();
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
