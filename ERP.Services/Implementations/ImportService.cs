using Amazon.S3.Model.Internal.MarshallTransformations;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class ImportService : IImportService
    {
        private readonly IGenericRepository<Contact> _repContacts;
        private readonly ITransactionService transactionService;
        private readonly IGenericRepository<Box> _repoBox;
        private readonly ISysRepository<Form> _repoForm;
        private readonly IGenericRepository<PaymentMethod> _repPaymentMethod;
        private readonly IGenericRepository<LedgerAccount> _repLedgerAccount;  
        private readonly IGenericRepository<HeadSemesters> _repHeadSemesters;
        private readonly IGenericRepository<FormLedgerAccount> _repTransactionReceipt;
        
        public ImportService(IGenericRepository<Contact> repContacts, IGenericRepository<HeadSemesters> _repHeadSemesters,IGenericRepository<FormLedgerAccount> _repTransactionReceipt, ISysRepository<Form> _repoForm, ITransactionService transactionService, IGenericRepository<Box> repoBox, IGenericRepository<PaymentMethod> repPaymentMethod, IGenericRepository<LedgerAccount> repLedgerAccount)
        {
            _repContacts = repContacts;
            this.transactionService = transactionService;
            _repoBox = repoBox;
            _repPaymentMethod = repPaymentMethod;
            _repLedgerAccount = repLedgerAccount;
            this._repoForm = _repoForm;
            this._repTransactionReceipt = _repTransactionReceipt;
            this._repHeadSemesters = _repHeadSemesters;
        }

        public async Task<List<RecipePayDto>> ImportRecipeService(ImportSemestersDto DataForImports)
        {

            var listRecipe = new List<RecipePayDto>();
            var listforms = GetFormData();
            foreach (var form in listforms)
            {
                for (int _Mesi = 1; _Mesi <= 12; _Mesi++)
                {

                    var data = await InsertRecipe(DataForImports.data, form, _Mesi, DataForImports.headSemesters.Year.Value);
                    if (data.Id.HasValue)
                    {
                        listRecipe.Add(data);
                    }
                }
            }

            await CreateOrUpdateHeadSemester(DataForImports);

            return listRecipe;
        }

        private async Task CreateOrUpdateHeadSemester(ImportSemestersDto DataForImports)
        {
            var newHeadSemester = new HeadSemesters()
            {
                InstitutionName = DataForImports.headSemesters.InstitutionName, // Nombre de la Institución
                Code = DataForImports.headSemesters.Code, // Código
                NumberOfSisters = DataForImports.headSemesters.NumberOfSisters.Value, // Número de Hermanas
                Country = DataForImports.headSemesters.Country, // País
                City = DataForImports.headSemesters.City, // Ciudad
                NumberOfEmployees = DataForImports.headSemesters.NumberOfEmployees.Value, // Número de Empleados
                Address = DataForImports.headSemesters.Address, // Dirección
                Phone = DataForImports.headSemesters.Phone, // Teléfono
                Fax = DataForImports.headSemesters.Fax, // Fax
                Year = DataForImports.headSemesters.Year.Value // Año

            };
            //quero buscar si existe este year 
            try
            {
                var existYear = await _repHeadSemesters.Find(x => x.Year == newHeadSemester.Year).FirstOrDefaultAsync();
                if (existYear == null)
                {
                    await _repHeadSemesters.InsertAsync(newHeadSemester);
                }
                else
                {
                    // lo contrario actualizo
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
            }
            catch (Exception ex)
            {

                throw;
            }
          
            
            //guardo 
            await _repHeadSemesters.SaveChangesAsync();
        }

        public record FormData(Guid Id, string Name, int TypeImpor, int TypeForm);

        public List<FormData> GetFormData()
        {
            var list = new List<FormData>();
            try
            {
                list.Add(new FormData(
              new Guid("b19343dc-c158-4220-9b05-31f793e339e4"),
              "Recibo de gastos",
              6, 11
          ));
                list.Add(new FormData(
                    new Guid("C619A80A-E7A8-4997-96C0-B03B12F597C2"),
                    "Recibo de ingresos",
                    4,10
                ));
            }
            catch (Exception ex)
            {

                throw;
            }
           
          

            return list;
           
        }


        //crea un metodo para _repoForm que retorne el id del formulario por el tipo
        public async Task<Form> GetFormIdByType(int _Type)
        {
            var form = await _repoForm.Find(x => x.TransactionsType == _Type).FirstOrDefaultAsync();
            if (form == null) return null;
            return form;
        }

        private async Task<RecipePayDto> InsertRecipe(List<CsvData> DataForImports, FormData formData  , int _Mes,   int year)
        {
          
            string nameContact = "Generico";
            var contact = await GetOrCreateContactForName(nameContact);
            var box = await GetFirstActiveBox();
            var paymentMethod = await GetFirstPaymentMethod();
            var _globalTotal = GetTotalByMonthAndYear(DataForImports, _Mes, formData.TypeImpor);
            var _DataForImports = await FilterDataByMonth(DataForImports, _Mes, formData, year);

            if (_DataForImports.Count == 0) return new RecipePayDto();
            var NewRecipe = new RecipePayDto();
            NewRecipe.FormId = formData.Id;
            NewRecipe.ContactId = contact.Id;
            NewRecipe.Date = GetLastDayOfMonth(_Mes, year);
            NewRecipe.BoxId = box.Id;
            NewRecipe.PaymentMethodId = paymentMethod.Id;
            NewRecipe.CurrencyId = box.CurrencyId.Value;
            NewRecipe.Type = formData.TypeForm;
            NewRecipe.GlobalTotal = _globalTotal;

            List<RecipeDetalles> RecipeDetalles = new List<RecipeDetalles>();
            foreach (var item in _DataForImports)
            {
                var RecipeDetalle = new RecipeDetalles();
                RecipeDetalle.Value = item.Paid;
                RecipeDetalle.referenceId = item.referenceId;
                RecipeDetalles.Add(RecipeDetalle);
            }
            NewRecipe.RecipeDetalles = RecipeDetalles;

             var dataSave = await transactionService.TransactionReceiptProcess(NewRecipe);
            return dataSave;
        }

        //crear metodo que busque o cree una cuenta contable con _repLedgerAccount
        public async Task<LedgerAccount> GetOrCreateLedgerAccount(CsvData csvData , int year, Guid FormId)
        {
            //Se tiene que agregar al formulario la cuenta que se creo
            var founLedgerAccount = await _repLedgerAccount.Find(x => x.Name == csvData.CUENTA).FirstOrDefaultAsync();
            if (founLedgerAccount != null) return founLedgerAccount;

            var newLedgerAccount = new LedgerAccount()
            {
                Code = csvData.COD,
                Name = csvData.CUENTA,
                Nature = GetMappedValue(csvData.TIPO),
                LocationStatusResult = csvData.TIPO,
                EntidadId = year

            };


          
            await _repLedgerAccount.InsertAsync(newLedgerAccount);
            await _repLedgerAccount.SaveChangesAsync();

            FormLedgerAccount transactionReceipt = new FormLedgerAccount()
            {
                LedgerAccountId = newLedgerAccount.Id,
                FormId = FormId
            };
             await _repTransactionReceipt.InsertAsync(transactionReceipt);
            await _repTransactionReceipt.SaveChangesAsync();
            return newLedgerAccount;
        }
        public int GetMappedValue(int number)
        {
            switch (number)
            {
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 1;
                case 5:
                    return 2;
                case 6:
                    return 2;
                default:
                    return 0;
            }
        }

        public record _RecipeDetalles(Guid referenceId, decimal Paid);
        public async Task<List<_RecipeDetalles>> FilterDataByMonth(List<CsvData> DataForImports, int _Mes, FormData formData, int year)
        {
            var _listReccor = new List<_RecipeDetalles>();

            switch (_Mes)
            {
                case 1:
                    var resultEnero = DataForImports.Where(x => x.ENERO > 0 && x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultEnero)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.ENERO));
                    }
                    return _listReccor;

                    
                case 2:
                    var resultFebrero = DataForImports.Where(x => x.FEBRERO > 0 && x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultFebrero)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.FEBRERO));
                    }
                    return _listReccor;
                case 3:
                    var resultMarzo = DataForImports.Where(x => x.MARZO > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultMarzo)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.MARZO));
                    }
                    return _listReccor;
                case 4:
                    var resultAbril = DataForImports.Where(x => x.ABRIL > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultAbril)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.ABRIL));
                    }
                    return _listReccor;
                case 5:
                    var resultMayo = DataForImports.Where(x => x.MAYO > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultMayo)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.MAYO));
                    }
                    return _listReccor;
                case 6:
                    var resultJunio = DataForImports.Where(x => x.JUNIO > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultJunio)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.JUNIO));
                    }
                    return _listReccor;
                case 7:
                    var resultJulio = DataForImports.Where(x => x.JULIO > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultJulio)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.JULIO));
                    }
                    return _listReccor;
                case 8:
                    var resultAgosto = DataForImports.Where(x => x.AGOSTO > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultAgosto)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.AGOSTO));
                    }
                    return _listReccor;
                case 9:
                    var resultSeptiembre = DataForImports.Where(x => x.SEPTIEMBRE > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultSeptiembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.SEPTIEMBRE));
                    }
                    return _listReccor;
                case 10:
                    var resultOctubre = DataForImports.Where(x => x.OCTUBRE > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultOctubre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.OCTUBRE));
                    }
                    return _listReccor;
                case 11:
                    var resultNoviembre = DataForImports.Where(x => x.NOVIEMBRE > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultNoviembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.NOVIEMBRE));
                    }
                    return _listReccor;
                case 12:
                    var resultDiciembre = DataForImports.Where(x => x.DICIEMBRE > 0&& x.TIPO == formData.TypeImpor).ToList();
                    foreach (var item in resultDiciembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item, year, formData.Id);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.DICIEMBRE));
                    }
                    return _listReccor;

            }

            return _listReccor;
        }

        //Crear metodo que devuelve el total por mes y ano de CsvData
        public decimal GetTotalByMonthAndYear(List<CsvData> DataForImports, int _Mes, int type)
        {
            switch (_Mes)
            {
                case 1:
                    return DataForImports.Where(x=> x.TIPO == type).Sum(x => x.ENERO);
                case 2:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.FEBRERO);
                case 3:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.MARZO);
                case 4:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.ABRIL);
                case 5:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.MAYO);
                case 6:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.JUNIO);
                case 7:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.JULIO);
                case 8:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.AGOSTO);
                case 9:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.SEPTIEMBRE);
                case 10:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.OCTUBRE);
                case 11:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.NOVIEMBRE);
                case 12:
                    return DataForImports.Where(x => x.TIPO == type).Sum(x => x.DICIEMBRE);

                default:
                    return 0;
            }

        }

        //crear metodo que por el mes y ano me devuelva el ultimo dia del mes
        public DateTime GetLastDayOfMonth(int month, int year)
        {
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return lastDayOfMonth;

        }

        //Crear metodo que devuelve la primera caja actica
        public async Task<Box> GetFirstActiveBox()
        {
            return await _repoBox.Find(x => x.IsActive == true).FirstOrDefaultAsync();
        }

        //crear metodo que devuelve el primer PaymentMethod
        public async Task<PaymentMethod> GetFirstPaymentMethod()
        {
            return await _repPaymentMethod.Find(x => x.IsActive == true).FirstOrDefaultAsync();
        }



        public async Task<Contact> GetOrCreateContactForName(string name)
        {
            var founContacta = await _repContacts.Find(x => x.Name == name).FirstOrDefaultAsync();
            if (founContacta != null) return founContacta;

            var newContact = new Contact()
            {
                Name = name
            };
            await _repContacts.InsertAsync(newContact);
            await _repContacts.SaveChangesAsync();
            return newContact;
        }


    }
}
