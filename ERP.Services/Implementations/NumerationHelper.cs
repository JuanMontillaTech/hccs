using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using ERP.Services.Interfaces;
using System;
namespace ERP.Services.Implementations
{
    public class NumerationHelper : INumerationHelper
    {
        private readonly IGenericRepository<Contact> _repContacts; 
        private readonly IGenericRepository<Form> _repForm;

        public NumerationHelper(
            IGenericRepository<Contact> repContacts,
            IGenericRepository<Form> repForm)
        {
            _repContacts = repContacts; 
            _repForm = repForm;
        }

        public async Task<string> GetNextNumerationSequence(Guid formId)
        {
            var rowForm = await _repForm.GetById(formId);
            if (rowForm.AllowSequence != null)
            {
                if (rowForm.AllowSequence.Value)
                {
                    rowForm.Sequence = rowForm.Sequence.Value + 1;
                    ;
                }
            }
            return rowForm.Prefix + rowForm.Sequence.ToString();
        }

     

        private async Task<Contact> GetContactWithNumeration(Guid? contactId)
        {
            return await _repContacts.Find(x => x.Id == contactId)
                .Include(x => x.Numeration)
                .FirstOrDefaultAsync();
        }

        

        public async Task<string> ValidateAndFetchNextTaxNumber(Guid? contactId)
        {
            var contact = await GetContactWithNumeration(contactId);

            if (contact?.Numeration?.Automatic == true)
            {
                return await GetNextTaxNumber(contact);
            }

            return null;
        }


        private async Task<string> GetNextTaxNumber( Contact contact)
        {
           contact.Numeration.Sequence++;
            return await Task.FromResult(contact.Numeration.Prefix + contact.Numeration.Sequence);
        }

       



      

        
 
    }
}