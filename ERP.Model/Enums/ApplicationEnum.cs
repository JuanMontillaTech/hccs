using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Enums
{
    public class ApplicationEnum
    {
        public enum WebPageSectionEnum
        {
            Undefined = 0,        //Sin Clasificacion
            General = 1,         //General
            Accounting = 2,       //Contabilidad
            Contact = 3,        //Contacto
            Address = 4,          //Direccion
            File = 5,           //Archivo
            ItemAndService = 6,  //Articulo/Servicio
            Tax = 7,           //Inpuesto
            Total = 8,          //Total
            AppliedDocuments = 9, //Documentos Aplicados
            Custom = 10,          //Personalizado
            Sales = 11,
            Purcher = 12,
            Commentary = 13,
            Log = 14
        }

        public enum RegisterNumberTypeEnum
        {
            IdentificationCard = 1, //Cedula
            NationalRegistryTaxpayers = 2, //RNC
                                           // Passport = 3, //Pasaporte
        }

        public enum ActionState
        {
            Create = 0,
            Update = 1,
            Delete = 2,
            Void = 3,
            Print = 4,
            Duplicate = 5,
            Detail = 6
        }

        public enum SubModuleSecurityEnum
        {
            AllowCreate = 0,
            AllowShowList = 1,
            AllowDetails = 3,
            AllowUpdate = 4,
            AllowDuplicate = 5,
            AllowDelete = 6,
            AllowVoid = 7,
            AllowPrint = 8
        }

        public enum PaymentTypeEnum
        {
            Cash = 1,
            Check = 2,
            ElectronicPayment = 3,
            CreditAndDebiCard = 4,
            Other = 5
        }

        public enum ActivityTypeEnum
        {
            Task = 1,
            Cita = 2,
            CallOfPhone = 3,
            Note = 4
        }

        public enum ActivityCallTypeEnum
        {
            Entry = 1,
            Out = 2,
        }

        public enum ActivityPriorityEnum
        {
            Baja = 1,
            Normal = 2,
            Alta = 3,
        }

        public enum DashBoardObjectTypeEnum
        {
            Graphic = 1,
            Calendar = 2,
            Activities = 3,
            Shortcut = 4,
            CustomReportList = 5,
            CustomReportChart = 6,
            ReportList = 7
        }

        public enum TransStatus
        {
            Sent = 0,
            Received = 1,
            PreparingInformation = 2,
            Complete = 3,
            Cancelled = 4
        }

        public enum MediationTransStatus
        {
            Sent = 0,
            Received = 1,
            PreparingInformation = 2,
            Complete = 3,
            Cancelled = 4
        }

        public enum DashBoardChartEnum
        {
            AccountsReceivable = 1,
            SalesLocations = 2,
            ExpensesMonth = 3,
            ExpenseLocations = 7,
            SalesMonth = 8,
            AccountPayable = 9,
            SalesMonthAndSeller = 10,
            SalesbyMonthAndPeriods = 11,
            ExpensesMonthOfPeriods = 12,
            CasesAssignedToEmployees = 18,
            CasesByType = 19,
            CasesByPriority = 20
        }

        public enum TypeValidationDGIIEnum
        {
            Null = 0,
            RNC = 1,
            NCF = 2,
        }

        public enum NotificationTypeAlertEnum
        {
            primary = 0,        //Sin Clasificacion
            secondary = 1,         //General
            success = 2,
            danger = 3,
            warning = 4,
            info = 5,
        }

        public enum NotificationState
        {
            Pending = 0,
            ignore = 1,
            Completed = 2,
        }

        public enum ModuleEnum
        {
            Null = 0,
            Sales = 1,
            Inventory = 2,
            Purchase = 3,
            Accounting = 5,
            HumanResource = 6,
            ProjectManagement = 7,
            Bank = 8,
            Manufacturing = 9,
            Crm = 10,
            Pos = 11,
            WebStore = 12,
            CrmAndWebStore = 13
        }

        public enum RMTransactionTypeEnum
        {
            QuotationRequest = 68,
            Quotation = 65,
            SalesOrders = 66,
            CreditInvoice = 71,
            CashInvoice = 70,
            CreditNote = 72,
            DebitNote = 73,
            CashReceipt = 74,
            ProFormaInvoice = 61,
            CustomerAdvance = 138,
            ApplicationDocument = 140,
            ScheduledCreditTransaction = 145
        }

        public enum MFTransactionTypeEnum
        {
            ProductionOrders = 194,
            DirectProduction = 195,
            DispatchRawMaterial = 196,
            ProductionReception = 197
        }

        public enum InvMovementTypeEnum
        {
            ReceptionInventory = 23,
            DispatchInventory = 25,
            InventoryAdjustment = 29,
            InventoryMovement = 26,
            InventoryMovementRequets = 141,
            DispatchRawMaterial = 196,
            ProductionReception = 197

        }

        public enum PMTransactionTypeEnum
        {
            QuoteRequest = 44,
            PurchaseOrder = 45,
            PurchaseInvoiceEntry = 47,
            PaymentVendor = 51,
            Expenses = 58,
            VendorAdvance = 139,
            Commission = 186,
            CreditNote = 48,
            DebitNote = 49
        }

        public enum SelectListValueEnum
        {
            RegisterNumberType = 1,
            PaymentType = 2,
            FiscalSequenceType = 3
        }

        public enum FiscalSequenceTypeEnum
        {
            Invoices = 1,
            Debits = 2,
            Credits = 3,
            Purchases = 4,
            Expense = 5
        }

        public enum ValidateDeleteRecordsEnum
        {
            Customer = 59,
            ProductsAndServices = 1,
            Employee = 111
        }

        public enum TaxRetentionCalculationMethodEnum
        {
            VAT = 1,
            EARNINGS = 2,
            SELECTIVE = 3,
            TIP = 4,
            OTHER = 5,
        }

        public enum AuthorationStatusEnum
        {
            Pending = 0,
            Authorized = 1,
            Rejected = 2,
            Other = 3
        }

        public enum SqlCustomReportEnum
        {
            Report = 0,
            Chart = 1,
        }

        public enum SqlCustomCharTypeEnum
        {
            Pie = 0,
            Line = 1,
            Doughnut = 2,
            Bar = 3,
            HorizontalBar = 4,
            PolarArea = 5
        }

        public enum TemplateControlEnum
        {
            Undefined = 0,
            TableAddress = 1,
            TableContacts = 2,
            RMTableDetailItem = 3,
            TableJournalTransAccount = 4,
            TablePaymentMethods = 5,
            TableTransDocuments = 6,
            TablePriceList = 7,
            TableAccessCompany = 8,
            TableUofMList = 9,
            PMTableDetailItem = 10,
            InvTableInventoryMovement = 11,
            InvTableItemMakers = 12,
            InvTablePhysicalInventoryCountItem = 13,
            TableApplicationDocuments = 14,
            RMPromotionDetail = 15,
            GLBalanceYearbyMonth = 16,
            TableItemComponents = 17,
            CrmTableCaseWorkOrders = 18,
            CrmTableWorkOrderlItem = 19,
            InvTableLandedCosts = 20,
            RMCommissionRule = 21,
            PMCommissionDoc = 22,
            BRBankReconciliationDocumets = 23,
            MFBOMTables = 24,
            MFProductionOrdersItem = 25
        }

        public enum RelationShipRolTypeEnum
        {
            Customer = 1,
            Vendor = 2,
            Employee = 3,
            SystemUser = 4
        }

        public enum HtmlInputTypeEnum
        {
            hidden = 0,
            text = 1,
            date = 2,
            email = 3,
            password = 4,
            number = 5,
            integer = 6,
            datetimelocal = 7
        }

        public enum DataTypesEnum
        {
            TextBox = 1,
            TextAre = 2,
            TextDate = 3,
            CheckBox = 4,
            SelectOption = 5,
            TableDetail = 6,
            TextBoxNumber = 7,
            TextBoxAutocomplete = 8,
            TextBoxInt = 9,
            TextEmail = 10,
            TextPassword = 11,
            TextOnlyNumber = 12,
            TextWebSite = 13,
            TextCommentary = 14,
            Picturebox = 15,
            TextBoxPhone = 16,
            DateTimeLocal = 17,
            RelationShipTextBoxAutocomplete = 18,
            AccountTextBoxAutocomplete = 19,
            TextBoxDateTimeLocal = 20,
            SelectEnumOption = 21,
        }

        public enum HtmlControlDataSourceEnum
        {
            Null = 0,
            AccountClasification = 1,
            TaxType = 2,
            GoodOrService = 3,
            CashBankAccount = 4,
            ClassTypeNcf = 5,
            TaxRetentionType = 6,
            HRRegisterNumberType = 7,
            CostMethod = 8,
            ProjectStateI = 9,
            PaymentTypes = 10,
            ActivityPriority = 11,
            CustomFieldDataType = 12,
            SalesStageType = 13,
            CurrencyGlobal = 14,
            BankAccount = 15,
            CashAccount = 16
        }

        public enum TransStatusIDEnum
        {
            Processed = 1,
            Invoiced = 2,
            Close = 3,
            Void = 4,
            Received = 5,
            PendingInvoice = 6,
            PendingPayment = 7,
            PartialPayment = 8,
            InvoicePaid = 9,
            PendingAuthorizationClient = 10,
            RejectedClient = 11,
            AuthorizedClient = 12,
        }

        public enum MFTransStatusIDEnum
        {
            Open = 1,
            Close = 2,
            Void = 4,
            Processed = 5
        }

        public enum CaseStatusIDEnum
        {
            PendingStart = 1,
            InProcess = 2,
            Pause = 3,
            Void = 4,
            PendingInvoice = 5,
            Invoiced = 6,
            Resolved = 7,
            Quotation = 8,
            PendingAuthorization = 9,
            AuthorizedbyClient = 10,
            RejectedClient = 11,
        }

        public enum HistoryAction
        {
            Add = 0,
            Modification = 1,
            Delete = 2,
            Void = 3,
            Print = 4,
            Duplicate = 5,
            AuthorizationAdd = 6,
            AuthorizationDuplicate = 7,
            AuthorizationNavigation = 8,
            AuthorizationModification = 9,
            AuthorizationPrint = 10,
            AuthorizationDelete = 11,
            AuthorizationVoid = 12,
            SendEmail = 13,
            File = 14
        }

        public enum SubModuleEnum
        {
            Null = 0,
            Inv_Item = 1,
            Sys_SecurityUser = 2,
            Sys_UofM = 3,
            Inv_UofMPlan = 4,
            Inv_WareHouse = 5,
            Inv_Location = 6,
            Inv_ItemCategories = 7,
            Inv_Make = 8,
            Inv_Model = 9,
            Inv_Color = 10,
            Inv_Size = 11,
            Inv_Family = 12,
            Sys_Tax = 13,
            Sys_TaxGroup = 14,
            Sys_Currency = 15,
            GL_Division = 16,
            GL_Account = 17,
            GL_CashAccount = 18,
            BR_Bank = 19,
            GL_Journal_Entry = 20,
            PM_Vendor = 21,
            Inv_PriceLevel = 22,
            Inv_ReceptionInventory = 23,
            RM_CashInvoice = 70,
            Inv_DispathInventory = 25,
            Inv_InventoryMovement = 26,
            Inv_PhysicalCountInventory = 28,
            Inv_AdjustmentInventory = 29,
            Sys_Rol = 30,
            Sys_DocFiscal = 31,
            Inv_ItemService = 33,
            Sys_Branch = 34,
            Sys_CompanyInf = 35,
            Inv_Option = 38,
            Sys_PaymentMethod = 39,
            PM_VendorCategory = 40,
            Sys_TaxRetention = 41,
            PM_VendorGroup = 42,
            PM_PmOptions = 43,
            PM_QuoteRequest = 44,
            PM_PurchaserOrder = 45,
            PM_ExpenseType = 46,
            PM_PurchaseInvoiceEntry = 47,
            PM_CreditNote = 48,
            PM_DebitNote = 49,
            BR_PayReq = 50,
            PM_PaymentVendor = 51,
            BR_NumSeqCheck = 52,
            Sys_CheckFormat = 53,
            PM_AgedPayables = 54,
            Sys_StorageFile = 57,
            PM_Expenses = 58,
            RM_Customer = 59,
            RM_CustomerGroup = 60,
            Sys_TaxRetentionGroup = 62,
            RM_CustomerCategory = 63,
            RM_Options = 64,
            RM_Quotation = 65,
            RM_SalesOrder = 66,
            Sys_PaymentTerm = 67,
            RM_QuotationRequest = 68,
            RM_CommissionRules = 69,
            RM_CustomerDiscountPayment = 24,
            RM_CreditInvoice = 71,
            RM_ProFormaInvoice = 61,
            RM_CreditNote = 72,
            RM_DebitNote = 73,
            RM_CashReceipt = 74,
            RM_SalesReturn = 75,
            RM_CancellarionType = 76,
            RM_AgedReceivables = 77,
            RM_StatementofAccount = 78,
            GL_GeneralLedger = 84,
            GL_TrialBalance = 85,
            GL_General_Sheet = 86,
            GL_ProfitLoss = 87,
            BR_DbCrBank = 88,
            BR_DepositBank = 89,
            BR_CheckReturn = 90,
            BR_BankConciliation = 91,
            GL_AccountingPeriod = 92,
            Sys_RevenueCode = 100,
            Sys_TaxRetentionCode = 101,
            Inv_LandedCosts = 102,
            Sys_CustomField = 104,
            Sys_City = 107,
            Sys_StateCity = 105,
            Sys_Country = 106,
            Sys_Nationality = 108,
            Sys_Contacts = 109,
            Sys_RegisterNumberType = 110,
            Gender = -999999,
            HR_Employee = 111,
            RM_ShipmentMethod = 112,
            Sys_SystemUser = 114,
            GL_ReportListAccounts = 115,
            PRM_Project = 116,
            PRM_ProjectGroup = 117,
            Inv_ItemProductReportStockZero = 118,
            Inv_ItemProductReportStock = 119,
            RM_AccountsReceivableSummed = 120,
            PM_AccountsPayableSummarized = 121,
            HR_ExpensesPayableSummarized = 122,
            RM_ReportListCreditInvoices = 123,
            RM_ReportListCashInvoices = 124,
            PRM_ReportListProjectSummary = 125,
            BR_CheckPrint = 126,
            Inv_Lot = 127,
            Sys_UserPreference = 128,
            Inv_ItemProductStockbyWarehouseLocationAndLot = 129,
            RM_ReportCashInvoiceDetail = 130,
            CRM_ActivityTask = 131,
            CRM_Activitie = 132,
            Sys_Migration = 133,
            RM_FiscalDoc608 = 134,
            RM_FiscalDoc607 = 135,
            PM_FiscalDoc606 = 136,
            Inv_ReportInventoryMovementAnalysis = 137,
            RM_CustomerAdvance = 138,
            PM_VendorAdvance = 139,
            RM_ApplicationSaleDocument = 140,
            Inv_InventoryMovementRequest = 141,
            RM_SalesbyPriceLevelSummed = 142,
            Inv_SalesForServicesSummed = 143,
            RM_Promotion = 144,
            RM_ScheduledCreditInvoice = 145,
            Inv_SalesForServicesForEmployee = 146,
            RM_SaleWithCost = 147,
            RM_SaleWithCostForItemProduct = 148,
            RM_CustomerBenefitSummary = 149,
            PM_ReportExpenses = 150,
            PM_ClassificationExpense = 151,
            Inv_ReCalculateInventoryCost = 152,
            Gl_AccountWithMovements = 153,
            GL_AccountBalanceTransfer = 154,
            RM_PosWorkShift = 155,
            RM_PosTrasactions = 156,
            RM_ReportCashRegisterSummary = 157,
            RM_ReportCashRegisterDetail = 158,
            Inv_ReceptionReportProduct = 163,
            Sys_AccountInformation = 164,
            Sys_DefaultReportFormal = 165,
            Inv_ItemKits = 166,
            MF_ManufacturingStage = 167,
            MF_BOMCategories = 168,
            MF_ManufacturingWorkCenter = 169,
            Inv_ReportItemsBelowReorderLevels = 170,
            RM_ExpiredInvoiceTransaction = 171,
            Inv_ReportPerfilItemProduct = 172,
            Crm_OpportunityLostReason = 173,
            Crm_OpportunitySalesStage = 174,
            Crm_OpportunityCompetitor = 175,
            Crm_CaseOrigin = 176,
            Crm_CasePriority = 177,
            Crm_CaseStatus = 178,
            Crm_CaseWorkType = 179,
            Crm_CaseType = 180,
            Crm_Opportunity = 181,
            Crm_Case = 182,
            Crm_OpportunityType = 183,
            Crm_CaseWorkOrder = 184,
            Crm_Option = 185,
            PM_Commission = 186,
            PM_ReportPurchaseInvoiceEntry = 188,
            RM_ReportSaleGeneral = 190,
            RM_ReportListCashReceipts = 191,
            RM_ReportSalesAndPaymentsSummary = 192,
            MF_BOM = 193,
            MF_ProductionOrder = 194,
            MF_DirectProduction = 195,
            MF_DispatchRawMaterial = 196,
            MF_ProductionReception = 197,
            MF_Options = 198,
            Crm_UnassignedCases = 199,
            Crm_CasesExpiration = 200,
            PM_ProductPurchaseRecommendation = 201
        }

        public enum JournalState
        {
            Create = 1,
            Void = 2,
            Post = 3
        }

        public enum MovementDocType
        {
            ItemEntry = 23,
            Dispath = 25,
            TransferInventory = 26,
            InventoryAdjustment = 29,
            SalesInvoice = 71,
            SalesReturn = 75
        }

        public enum JournalType
        {
            JournalEntry = 20,
            InvReceptionInventory = 23,
            DispatchInventory = 25,
            InventoryMovement = 26,
            InventoryAdjustment = 29,
            PurchaseInvoiceEntry = 47,
            PMCreditNote = 48,
            PMDebitNote = 49,
            PaymentVendor = 51,
            BRBankTransfer = 53,
            PMExpenses = 58,
            RMProFormaInvoice = 61,
            RMCreditInvoice = 71,
            RMCashInvoice = 70,
            RMCreditNote = 72,
            RMDebitMemo = 73,
            RMCashReceipt = 74,
            RMSalesReturns = 75,
            BRDbCrBank = 88,
            BRDepositBank = 89,
            BRCheckReturns = 90,
            RMCustomerAdvance = 138,
            PMVendorAdvance = 139,
            RMApplicationOfDocument = 140,
            PMCommission = 186
        }

        public enum PeriodState
        {
            Close = 0,
            Open = 1,
            NoExist = 2
        }

        public enum PaymentEntrySourceDocType
        {
            Invoice = 71,
            Receipt = 74
        }

        public enum ItemType
        {
            Product = 1,
            Service = 2,
            Kit = 3
        }

        public enum ReceptionStatusTypes
        {
            Pending = 1,
            Partial = 2,
            Hold = 3,
            Closed = 4,
            Completed = 5
        }

        public enum VoucherType
        {
            NotSpecified = 0,
            Merchandise = 1,
            AmountofExpense = 2
        }

        public enum ViewTypeEnum
        {
            List = 0,
            Create = 1,
            Details = 2,
            Clone = 3,
            Edit = 4,
            ReportList = 5
        }

        public enum ListFormStructEnum
        {
            Contact = 1
        }

        public enum LanguageEnum
        {
            es_DO = 0,
            en_EN = 1
        }

        public enum SubModuleTemplateTypeEnum
        {
            Normal = 0,
            CustomForm = 1,
            FormExpress = 2,
        }

        public enum SubModuleTypeEnum
        {
            Normal = 0,
            Transaction = 1,
            Browser = 2,
            SqlReport = 3
        }


        public enum CostMethodEnum
        {
            AverageCost = 0
        }

    }

}
