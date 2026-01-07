using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Country
{
    public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
    {
        public CreateCountryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(2);

            RuleFor(x => x.Population)
                .NotEmpty()
                .LessThan(0);

            RuleFor(x => x.Area)
                .NotEmpty()
                .LessThan(0);
        }
    }
}
