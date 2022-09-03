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

namespace ERP.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public readonly IGenericRepository<Transactions> _RepoTrasacion;

        public TransactionService(IGenericRepository<Transactions> repoTrasacion)
        {
            _RepoTrasacion = repoTrasacion;
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
            //Insert DB
            //Create accounting detalles
            return transactions;
        }
    }
}
