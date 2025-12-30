using Application.DTOs;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
        private readonly IUserRepository _userRepository;

        public RegisterDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters")
                 .MustAsync(BeUniqueEmail).WithMessage("Email already exists"); 
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters");

            // .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            //.Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            //.Matches(@"[0-9]").WithMessage("Password must contain at least one number")
            //.Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.ExistsAsync(string.Empty, email, cancellationToken);
        }

    }
}
