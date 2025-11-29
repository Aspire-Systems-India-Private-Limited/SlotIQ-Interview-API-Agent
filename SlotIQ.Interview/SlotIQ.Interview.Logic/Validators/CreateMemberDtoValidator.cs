using FluentValidation;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Validators;

public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberDtoValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ErrorMessages.Required("UserName"))
            .MinimumLength(5).WithMessage(ErrorMessages.MinLength("UserName", 5))
            .MaximumLength(100).WithMessage(ErrorMessages.MaxLength("UserName", 100))
            .Matches("^[a-zA-Z0-9._-]+$").WithMessage("UserName can only contain letters, numbers, dots, underscores, and hyphens")
            .MustAsync(async (userName, cancellation) => !await UserNameExists(userName))
            .WithMessage(ErrorMessages.DuplicateUserName);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ErrorMessages.Required("FirstName"))
            .MinimumLength(2).WithMessage(ErrorMessages.MinLength("FirstName", 2))
            .MaximumLength(50).WithMessage(ErrorMessages.MaxLength("FirstName", 50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ErrorMessages.Required("LastName"))
            .MinimumLength(2).WithMessage(ErrorMessages.MinLength("LastName", 2))
            .MaximumLength(50).WithMessage(ErrorMessages.MaxLength("LastName", 50));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ErrorMessages.Required("Password"))
            .MinimumLength(8).WithMessage(ErrorMessages.MinLength("Password", 8));

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage(ErrorMessages.Required("EmailAddress"))
            .EmailAddress().WithMessage(ErrorMessages.InvalidEmail)
            .Matches("^[a-zA-Z0-9._%+-]+@aspiresys\\.com$").WithMessage(ErrorMessages.InvalidEmailDomain)
            .MustAsync(async (email, cancellation) => !await EmailExists(email))
            .WithMessage(ErrorMessages.DuplicateEmailAddress);

        RuleFor(x => x.PhoneNumber)
            .Matches("^[0-9]{10}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be exactly 10 digits");

        RuleFor(x => x.PracticeID)
            .NotEmpty().WithMessage(ErrorMessages.Required("PracticeID"));

        RuleFor(x => x.UpdatedBy)
            .NotEmpty().WithMessage(ErrorMessages.Required("UpdatedBy"));
    }

    private async Task<bool> UserNameExists(string userName)
    {
        var result = await _memberRepository.GetByUserNameAsync(userName);
        return result.IsSuccess;
    }

    private async Task<bool> EmailExists(string email)
    {
        var result = await _memberRepository.GetByEmailAsync(email);
        return result.IsSuccess;
    }
}
