using Amazon.S3.Model.Internal.MarshallTransformations;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public Guid FormId;
        
        public ImportService(IGenericRepository<Contact> repContacts, ISysRepository<Form> _repoForm, ITransactionService transactionService, IGenericRepository<Box> repoBox, IGenericRepository<PaymentMethod> repPaymentMethod, IGenericRepository<LedgerAccount> repLedgerAccount)
        {
            _repContacts = repContacts;
            this.transactionService = transactionService;
            _repoBox = repoBox;
            _repPaymentMethod = repPaymentMethod;
            _repLedgerAccount = repLedgerAccount;
            this._repoForm = _repoForm;
        }

        public async Task<List<RecipePayDto>> ImportRecipeService(List<CsvData> DataForImports)
        {
         
            var listRecipe = new List<RecipePayDto>();
            for (int _Typei = 9; _Typei <= 20; _Typei++)
            {
                   FormId = await GetFormIdByType(_Typei);
               

                if (FormId != Guid.Empty)
                {

             
                for (int _Mesi = 1; _Typei <= 12; _Typei++)
                {
                 
                    var data = await InsertRecipe(DataForImports, _Typei, _Mesi, FormId);
                    listRecipe.Add(data);
                }
                }
            }
           

            return listRecipe;
        }
        //crea un metodo para _repoForm que retorne el id del formulario por el tipo
        public async Task<Guid> GetFormIdByType(int _Type)
        {
            var form = await _repoForm.Find(x => x.TransactionsType == _Type).FirstOrDefaultAsync();

            return form.Id;
        }

        private async Task<RecipePayDto> InsertRecipe(List<CsvData> DataForImports, int _Type, int _Mes, Guid FormId)
        {
            var FirstData = DataForImports.FirstOrDefault();
            string nameContact = "GENERICO_" + _Mes + "_" + FirstData.TIEMPO.ToString();
            var contact = await GetOrCreateContactForName(nameContact);
            var box = await GetFirstActiveBox();
            var paymentMethod = await GetFirstPaymentMethod();
            var _globalTotal = GetTotalByMonthAndYear(DataForImports, _Mes, _Type);
            var _DataForImports = await FilterDataByMonth(DataForImports, _Mes, _Type);

            if (_DataForImports.Count == 0) return new RecipePayDto();
            var NewRecipe = new RecipePayDto();
            NewRecipe.FormId = FormId;
            NewRecipe.ContactId = contact.Id;
            NewRecipe.Date = GetLastDayOfMonth(_Mes, (int)FirstData.TIEMPO);
            NewRecipe.BoxId = box.Id;
            NewRecipe.PaymentMethodId = paymentMethod.Id;
            NewRecipe.CurrencyId = box.CurrencyId.Value;
            NewRecipe.Type = _Type;
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
        public async Task<LedgerAccount> GetOrCreateLedgerAccount(string _code, string _name, int Type)
        {
            //Se tiene que agregar al formulario la cuenta que se creo
            var founLedgerAccount = await _repLedgerAccount.Find(x => x.Code == _code).FirstOrDefaultAsync();
            if (founLedgerAccount != null) return founLedgerAccount;

            var newLedgerAccount = new LedgerAccount()
            {
                Code = _code,
                Name = _name,
                Nature = GetMappedValue(Type),
                LocationStatusResult = Type
            };
            await _repLedgerAccount.InsertAsync(newLedgerAccount);
            await _repLedgerAccount.SaveChangesAsync();
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
        //crear metodo que filtre DataForImports con un switch por mes
        public async Task<List<_RecipeDetalles>> FilterDataByMonth(List<CsvData> DataForImports, int _Mes, int type)
        {
            var _listReccor = new List<_RecipeDetalles>();

            switch (_Mes)
            {
                case 1:
                    var resultEnero = DataForImports.Where(x => x.ENERO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultEnero)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.ENERO));
                    }
                    return _listReccor;

                    
                case 2:
                    var resultFebrero = DataForImports.Where(x => x.FEBRERO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultFebrero)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.FEBRERO));
                    }
                    return _listReccor;
                case 3:
                    var resultMarzo = DataForImports.Where(x => x.MARZO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultMarzo)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.MARZO));
                    }
                    return _listReccor;
                case 4:
                    var resultAbril = DataForImports.Where(x => x.ABRIL > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultAbril)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.ABRIL));
                    }
                    return _listReccor;
                case 5:
                    var resultMayo = DataForImports.Where(x => x.MAYO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultMayo)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.MAYO));
                    }
                    return _listReccor;
                case 6:
                    var resultJunio = DataForImports.Where(x => x.JUNIO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultJunio)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.JUNIO));
                    }
                    return _listReccor;
                case 7:
                    var resultJulio = DataForImports.Where(x => x.JULIO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultJulio)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.JULIO));
                    }
                    return _listReccor;
                case 8:
                    var resultAgosto = DataForImports.Where(x => x.AGOSTO > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultAgosto)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.AGOSTO));
                    }
                    return _listReccor;
                case 9:
                    var resultSeptiembre = DataForImports.Where(x => x.SEPTIEMBRE > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultSeptiembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.SEPTIEMBRE));
                    }
                    return _listReccor;
                case 10:
                    var resultOctubre = DataForImports.Where(x => x.OCTUBRE > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultOctubre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.OCTUBRE));
                    }
                    return _listReccor;
                case 11:
                    var resultNoviembre = DataForImports.Where(x => x.NOVIEMBRE > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultNoviembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
                        _listReccor.Add(new _RecipeDetalles(accounting.Id, item.NOVIEMBRE));
                    }
                    return _listReccor;
                case 12:
                    var resultDiciembre = DataForImports.Where(x => x.DICIEMBRE > 0 && x.TIPO == type).ToList();
                    foreach (var item in resultDiciembre)
                    {
                        var accounting = await GetOrCreateLedgerAccount(item.CUENTA, item.CUENTA, item.TIPO);
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
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
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
