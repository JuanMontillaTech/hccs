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
            CreateMap<EventDto, Event>().ReverseMap();
            CreateMap<PrintingFormDto, PrintingForm>().ReverseMap();
            CreateMap<ReportQueryDto, ReportQuery>().ReverseMap();
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
            CreateMap<FormRule, FormRuleDto>().ReverseMap();
            CreateMap<SysUser, Sys_UserDto>().ReverseMap();
            CreateMap<UserRoll, UserRollDto>().ReverseMap();
            CreateMap<UserRoll, UserRollDetallisDto>().ReverseMap(); 
            CreateMap<ConceptDto, Concept>().ReverseMap();
            CreateMap<TransactionsContactDto, Contact>().ReverseMap();      
            CreateMap<LedgerAccountDto, LedgerAccount>().ReverseMap();
            CreateMap<JournalDto, Journal>().ReverseMap();
            CreateMap<JournaDetails, JournaDetailsDto>().ReverseMap();
            CreateMap<Transactions, TransactionsDto>().ReverseMap();
            CreateMap<Transactions, TransactionsContactDto>().ReverseMap();
            CreateMap<TransactionsDetails, TransactionsDetailsDto>().ReverseMap();
            CreateMap<TransactionStatus, TransactionStatusDto>().ReverseMap();
            CreateMap<Numeration, NumerationDto>().ReverseMap();
            CreateMap<TransactionReceipt, TransactionReceiptDto>()
                //.ForMember(x => x.TransactionReceiptDetails, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.TransactionReceiptDetail, opt => opt.Ignore());
            CreateMap<ConfigurationPurchase, ConfigurationPurchaseDto>().ReverseMap();
            CreateMap<Taxes, TaxesDto>().ReverseMap(); 
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap(); 
            CreateMap<ConfigurationReport, ConfigurationReportIdDto>().ReverseMap();
            CreateMap<ConfigurationReport, ConfigurationReportDto>().ReverseMap(); 
            CreateMap<Payroll, PayrollDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap(); 
            CreateMap<Company, CompanyDto>().ReverseMap(); 
            CreateMap<ConfigurationSell, ConfigurationSellDto>().ReverseMap();
            CreateMap<Company, CompanyIdDto>().ReverseMap(); 
            CreateMap<TransactionLocation, TransactionLocationDto>().ReverseMap(); 
            CreateMap<FileManager, FileManagerDto>().ReverseMap(); 
            CreateMap<ConceptElement, ConceptElementDto>().ReverseMap(); 
            CreateMap<TransactionLocationTransaction, TransactionLocationTransactionDto>().ReverseMap(); 
            CreateMap<TransactionsDetailsElementType, TransactionsDetailsElementTypeDto>().ReverseMap(); 
            CreateMap<TransactionsDetailsElement, TransactionsDetailsElementDto>().ReverseMap(); 
            CreateMap<GroupTaxesTaxes, GroupTaxesTaxesDto>().ReverseMap(); 
            CreateMap<GroupTaxes, GroupTaxesDto>().ReverseMap(); 
            CreateMap<TransactionReceiptDetails, TransactionReceiptDetailsDto>().ReverseMap();
            CreateMap<TransactionReceipt, TransactionReceiptDto>().ReverseMap();
            CreateMap<TransaccionAccountForm, TransaccionAccountFormDto>().ReverseMap(); 
            CreateMap<Box, BoxDto>().ReverseMap(); 
            CreateMap<SysCompany, SysCompanyDto>().ReverseMap(); 
            CreateMap<SysUserCompany, SysUserCompanyDto>().ReverseMap(); 
            CreateMap<FormLedgerAccount, FormLedgerAccountDto>().ReverseMap();
            CreateMap<LedgerAccountCategory, LedgerAccountCategoryDto>().ReverseMap(); 
        }
    }
}
