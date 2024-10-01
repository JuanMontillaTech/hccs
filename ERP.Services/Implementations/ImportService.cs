using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class ImportService : IImportService
    {
        private readonly IGenericRepository<Contact> _repContacts;
        private readonly ITransactionService _transactionService;
        private readonly IGenericRepository<Box> _repoBox;
        private readonly ISysRepository<Form> _repoForm;
        private readonly IGenericRepository<PaymentMethod> _repPaymentMethod;
        private readonly IGenericRepository<LedgerAccount> _repLedgerAccount;
        private readonly IGenericRepository<HeadSemesters> _repHeadSemesters;
        private readonly IGenericRepository<FormLedgerAccount> _repTransactionReceipt;

        public ImportService(
            IGenericRepository<Contact> repContacts,
            IGenericRepository<HeadSemesters> repHeadSemesters,
            IGenericRepository<FormLedgerAccount> repTransactionReceipt,
            ISysRepository<Form> repoForm,
            ITransactionService transactionService,
            IGenericRepository<Box> repoBox,
            IGenericRepository<PaymentMethod> repPaymentMethod,
            IGenericRepository<LedgerAccount> repLedgerAccount)
        {
            _repContacts = repContacts;
            _transactionService = transactionService;
            _repoBox = repoBox;
            _repPaymentMethod = repPaymentMethod;
            _repLedgerAccount = repLedgerAccount;
            _repoForm = repoForm;
            _repTransactionReceipt = repTransactionReceipt;
            _repHeadSemesters = repHeadSemesters;
        }

        public async Task<List<RecipePayDto>> ImportRecipeService(ImportSemestersDto dataForImports)
        {
            var listRecipe = new List<RecipePayDto>();
            var listForms = GetFormData();
            var tasks = listForms.SelectMany(form => Enumerable.Range(1, 12).Select(async month =>
            {
                var data = await InsertRecipe(dataForImports.data, form, month, dataForImports.headSemesters.Year.Value);
                if (data.Id.HasValue)
                {
                    listRecipe.Add(data);
                }
            })).ToList();

            await Task.WhenAll(tasks);
            await CreateOrUpdateHeadSemester(dataForImports);

            return listRecipe;
        }

        private async Task CreateOrUpdateHeadSemester(ImportSemestersDto dataForImports)
        {
            var newHeadSemester = new HeadSemesters
            {
                InstitutionName = dataForImports.headSemesters.InstitutionName,
                Code = dataForImports.headSemesters.Code,
                NumberOfSisters = dataForImports.headSemesters.NumberOfSisters.Value,
                Country = dataForImports.headSemesters.Country,
                City = dataForImports.headSemesters.City,
                NumberOfEmployees = dataForImports.headSemesters.NumberOfEmployees.Value,
                Address = dataForImports.headSemesters.Address,
                Phone = dataForImports.headSemesters.Phone,
                Fax = dataForImports.headSemesters.Fax,
                Year = dataForImports.headSemesters.Year.Value
            };

            var existYear = await _repHeadSemesters.Find(x => x.Year == newHeadSemester.Year).FirstOrDefaultAsync();
            if (existYear == null)
            {
                await _repHeadSemesters.InsertAsync(newHeadSemester);
            }
            else
            {
                existYear.InstitutionName = newHeadSemester.InstitutionName;
                existYear.Code = newHeadSemester.Code;
                existYear.NumberOfSisters = newHeadSemester.NumberOfSisters;
                existYear.Country = newHeadSemester.Country;
                existYear.City = newHeadSemester.City;
                existYear.NumberOfEmployees = newHeadSemester.NumberOfEmployees;
                existYear.Address = newHeadSemester.Address;
                existYear.Phone = newHeadSemester.Phone;
                existYear.Fax = newHeadSemester.Fax;
                existYear.Year = newHeadSemester.Year;
                await _repHeadSemesters.Update(existYear);
            }

            await _repHeadSemesters.SaveChangesAsync();
        }

        public record FormData(Guid Id, string Name, int TypeImpor, int TypeForm);

        public List<FormData> GetFormData()
        {
            return new List<FormData>
            {
                new FormData(new Guid("b19343dc-c158-4220-9b05-31f793e339e4"), "Recibo de gastos", 6, 11),
                new FormData(new Guid("C619A80A-E7A8-4997-96C0-B03B12F597C2"), "Recibo de ingresos", 4, 10)
            };
        }

        public async Task<Form> GetFormIdByType(int type)
        {
            return await _repoForm.Find(x => x.TransactionsType == type).FirstOrDefaultAsync();
        }

        private async Task<RecipePayDto> InsertRecipe(List<CsvData> dataForImports, FormData formData, int month, int year)
        {
            var contact = await GetOrCreateContactForName("Generico");
            var box = await GetFirstActiveBox();
            var paymentMethod = await GetFirstPaymentMethod();
            var globalTotal = GetTotalByMonthAndYear(dataForImports, month, formData.TypeImpor);
            var filteredData = await FilterDataByMonth(dataForImports, month, formData, year);

            if (!filteredData.Any()) return new RecipePayDto();

            var newRecipe = new RecipePayDto
            {
                FormId = formData.Id,
                ContactId = contact.Id,
                Date = GetLastDayOfMonth(month, year),
                BoxId = box.Id,
                PaymentMethodId = paymentMethod.Id,
                CurrencyId = box.CurrencyId.Value,
                Type = formData.TypeForm,
                GlobalTotal = globalTotal,
                RecipeDetalles = filteredData.Select(item => new RecipeDetalles
                {
                    Value = item.Paid,
                    referenceId = item.referenceId
                }).ToList()
            };

            return await _transactionService.TransactionReceiptProcess(newRecipe);
        }

        public async Task<LedgerAccount> GetOrCreateLedgerAccount(CsvData csvData, int year, Guid formId)
        {
            var foundLedgerAccount = await _repLedgerAccount.Find(x => x.Name == csvData.CUENTA).FirstOrDefaultAsync();
            if (foundLedgerAccount != null) return foundLedgerAccount;

            var newLedgerAccount = new LedgerAccount
            {
                Code = csvData.COD,
                Name = csvData.CUENTA,
                Nature = GetMappedValue(csvData.TIPO),
                LocationStatusResult = csvData.TIPO,
                EntidadId = year
            };

            await _repLedgerAccount.InsertAsync(newLedgerAccount);
            await _repLedgerAccount.SaveChangesAsync();

            var transactionReceipt = new FormLedgerAccount
            {
                LedgerAccountId = newLedgerAccount.Id,
                FormId = formId
            };

            await _repTransactionReceipt.InsertAsync(transactionReceipt);
            await _repTransactionReceipt.SaveChangesAsync();

            return newLedgerAccount;
        }

        public int GetMappedValue(int number)
        {
            return number switch
            {
                1 => 2,
                2 => 1,
                3 => 2,
                4 => 1,
                5 => 2,
                6 => 2,
                _ => 0
            };
        }

        public record _RecipeDetalles(Guid referenceId, decimal Paid);

        public async Task<List<_RecipeDetalles>> FilterDataByMonth(List<CsvData> dataForImports, int month, FormData formData, int year)
        {
            var monthData = month switch
            {
                1 => dataForImports.Where(x => x.ENERO > 0 && x.TIPO == formData.TypeImpor),
                2 => dataForImports.Where(x => x.FEBRERO > 0 && x.TIPO == formData.TypeImpor),
                3 => dataForImports.Where(x => x.MARZO > 0 && x.TIPO == formData.TypeImpor),
                4 => dataForImports.Where(x => x.ABRIL > 0 && x.TIPO == formData.TypeImpor),
                5 => dataForImports.Where(x => x.MAYO > 0 && x.TIPO == formData.TypeImpor),
                6 => dataForImports.Where(x => x.JUNIO > 0 && x.TIPO == formData.TypeImpor),
                7 => dataForImports.Where(x => x.JULIO > 0 && x.TIPO == formData.TypeImpor),
                8 => dataForImports.Where(x => x.AGOSTO > 0 && x.TIPO == formData.TypeImpor),
                9 => dataForImports.Where(x => x.SEPTIEMBRE > 0 && x.TIPO == formData.TypeImpor),
                10 => dataForImports.Where(x => x.OCTUBRE > 0 && x.TIPO == formData.TypeImpor),
                11 => dataForImports.Where(x => x.NOVIEMBRE > 0 && x.TIPO == formData.TypeImpor),
                12 => dataForImports.Where(x => x.DICIEMBRE > 0 && x.TIPO == formData.TypeImpor),
                _ => Enumerable.Empty<CsvData>()
            };

            var tasks = monthData.Select(async item =>
            {
                var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                decimal monthValue = month switch
                {
                    1 => item.ENERO,
                    2 => item.FEBRERO,
                    3 => item.MARZO,
                    4 => item.ABRIL,
                    5 => item.MAYO,
                    6 => item.JUNIO,
                    7 => item.JULIO,
                    8 => item.AGOSTO,
                    9 => item.SEPTIEMBRE,
                    10 => item.OCTUBRE,
                    11 => item.NOVIEMBRE,
                    12 => item.DICIEMBRE,
                    _ => 0
                };
                return new _RecipeDetalles(accounting.Id, monthValue);
            });


            return (await Task.WhenAll(tasks)).ToList();
        }

        public decimal GetTotalByMonthAndYear(List<CsvData> dataForImports, int month, int type)
        {
            return month switch
            {
                1 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.ENERO),
                2 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.FEBRERO),
                3 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.MARZO),
                4 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.ABRIL),
                5 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.MAYO),
                6 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.JUNIO),
                7 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.JULIO),
                8 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.AGOSTO),
                9 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.SEPTIEMBRE),
                10 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.OCTUBRE),
                11 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.NOVIEMBRE),
                12 => dataForImports.Where(x => x.TIPO == type).Sum(x => x.DICIEMBRE),
                _ => 0
            };
        }

        public DateTime GetLastDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        public async Task<Box> GetFirstActiveBox()
        {
            return await _repoBox.Find(x => x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<PaymentMethod> GetFirstPaymentMethod()
        {
            return await _repPaymentMethod.Find(x => x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Contact> GetOrCreateContactForName(string name)
        {
            var foundContact = await _repContacts.Find(x => x.Name == name).FirstOrDefaultAsync();
            if (foundContact != null) return foundContact;

            var newContact = new Contact { Name = name };
            await _repContacts.InsertAsync(newContact);
            await _repContacts.SaveChangesAsync();
            return newContact;
        }
    }
}