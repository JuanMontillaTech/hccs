using ERP.Domain.Dtos;
using FluentValidation;

namespace ERP.API.ValidatorDto
{
   
    public class BankValidator : AbstractValidator<BankDto>
    {
        public BankValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido"); ;
            
        }

       
    }
}
