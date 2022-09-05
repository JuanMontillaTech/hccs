using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Services.Constants;
using Org.BouncyCastle.Crypto;
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public readonly IGenericRepository<Transactions> _RepoTrasacion;
        public readonly IGenericRepository<TransactionsDetails> _RepoTrasacionDetails;

        public TransactionService(IGenericRepository<Transactions> repoTrasacion, IGenericRepository<TransactionsDetails> repoTrasacionDetails)
        {
            _RepoTrasacion = repoTrasacion;
            _RepoTrasacionDetails = repoTrasacionDetails;
        }

        public async Task<Transactions> TransactionProcess (Transactions transactions)
        {
            //Determination what trasanction is
            switch (transactions.TransactionsType)
            {
                case (int)Constants.Constants.Document.InvoiceCredit:

                break;
                 
            }

            //Valide data is correct
            //programing procces to Create
            if (transactions.Id == Guid.Empty)
            {

                await _RepoTrasacion.Insert(transactions);
                _RepoTrasacion.Save();


            }
            else
            {
                await _RepoTrasacion.Update(transactions);

                 
                var _TransantionDetealleForDelete = await _RepoTrasacionDetails.Find(x => x.TransactionsId == transactions.Id).ToListAsync();
               

                foreach (var item in _TransantionDetealleForDelete)
                {
                var resulDelte= await _RepoTrasacionDetails.Delete(item.Id);
                   await _RepoTrasacionDetails.SaveChangesAsync();
                }
                var TransactionsDetailsList = transactions.TransactionsDetails;
                transactions.TransactionsDetails = TransactionsDetailsList;
                await _RepoTrasacionDetails.InsertArray(transactions.TransactionsDetails);

                await _RepoTrasacion.SaveChangesAsync();
              


            }
            //Insert DB
            //Create accounting detalles
            return transactions;
        }
    }
}
