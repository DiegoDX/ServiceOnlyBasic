using Application.DTOs;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.City
{
    public class CreateCityDtoValidator : AbstractValidator<CreateCityDto>
    {
        private readonly ICountryRepository _countryRepository;

        public CreateCityDtoValidator(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Population)
                .NotEmpty()
                .LessThan(0);

            RuleFor(x => x.Area)
                .NotEmpty()
                .LessThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CountryId)
                .NotNull().WithMessage("Country Id is required")
                .NotEqual(Guid.Empty).WithMessage("Country ID cannot be empty")
                .MustAsync(CountryExists).WithMessage("Country does not exist");


        }

        private async Task<bool> CountryExists(Guid countryId, CancellationToken cancellationToken)
        {
            return await _countryRepository.ExistsAsync(x => x.Id == countryId, cancellationToken);
        }
    }
}
