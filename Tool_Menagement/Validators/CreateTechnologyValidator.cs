using FluentValidation;
using Tool_Menagement.Models;
namespace Tool_Menagement.Validators
{
    public class CreateTechnologyValidator:AbstractValidator<TechnologiumViewModel>
    {
        public CreateTechnologyValidator() 
        {
            RuleFor(x => x.CzasPracy)
                .GreaterThanOrEqualTo(0).WithMessage("Czas pracy musi być liczbą większą lub równą 0");
        }
    }
}
