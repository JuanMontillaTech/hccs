using ERP.Domain.Entity;
using ERP.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class NumerationServices : INumerationService
    {
        public readonly IGenericRepository<Numeration> numerationRepository;

        public NumerationServices(IGenericRepository<Numeration> numerationRepository)
        {
            this.numerationRepository = numerationRepository;
        }

        public async Task<IEnumerable<Numeration>> GetAllByDocumentType() => await numerationRepository.GetAll();


        public async Task<string> GetNextDocumentAsync(Guid id)
        {
            var AllNumber = await GetAllByDocumentType();

            var Filter = AllNumber.Where(x =>  x.Id == id).FirstOrDefault();
            if (Filter == null) return "No encontrado";


            string strFormat = String.Format("{0}{1}", Filter.Prefix, Filter.Sequence + 1);

            return strFormat;
        }

        public async Task SaveNextNumber(Guid id)
        {
            var AllNumber = await numerationRepository.GetById(id);

            if (AllNumber != null)
            {


                AllNumber.Sequence = AllNumber.Sequence + 1;

                await numerationRepository.SaveChangesAsync();
            }
             

        }
    }
}
