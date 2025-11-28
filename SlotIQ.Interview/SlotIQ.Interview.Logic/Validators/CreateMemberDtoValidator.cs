using FluentValidation;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Validators;

/// <summary>
/// Validator for CreateMemberDto
/// </summary>
public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberDtoValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;

        // UserName validation
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ErrorMessages.UserNameRequired)
            .Length(5, 100).WithMessage(ErrorMessages.UserNameLength)
            .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage(ErrorMessages.UserNameFormat)
            .MustAsync(async (userName, cancellation) => !await _memberRepository.UserNameExistsAsync(userName))
            .WithMessage(ErrorMessages.DuplicateUserName);

        // FirstName validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ErrorMessages.FirstNameRequired)
            .Length(2, 50).WithMessage(ErrorMessages.FirstNameLength);

        // LastName validation
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ErrorMessages.LastNameRequired)
            .Length(2, 50).WithMessage(ErrorMessages.LastNameLength);

        // EmailID validation
        RuleFor(x => x.EmailID)
            .NotEmpty().WithMessage(ErrorMessages.EmailAddressRequired)
            .EmailAddress().WithMessage(ErrorMessages.EmailAddressDomain)
            .Must(email => email.EndsWith("@aspiresys.com", StringComparison.OrdinalIgnoreCase))
            .WithMessage(ErrorMessages.EmailAddressDomain)
            .MustAsync(async (email, cancellation) => !await _memberRepository.EmailExistsAsync(email))
            .WithMessage(ErrorMessages.DuplicateEmailAddress);

        // PhoneNumber validation (optional)
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[0-9]{10}$").WithMessage(ErrorMessages.PhoneNumberFormat)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .MustAsync(async (phoneNumber, cancellation) =>
            {
                if (string.IsNullOrEmpty(phoneNumber)) return true;
                return !await _memberRepository.PhoneNumberExistsAsync(phoneNumber);
            })
            .WithMessage(ErrorMessages.DuplicatePhoneNumber);

        // RoleID validation
        RuleFor(x => x.RoleID)
            .IsInEnum().WithMessage(ErrorMessages.RoleInvalid);

        // PracticeID validation
        RuleFor(x => x.PracticeID)
            .NotEmpty().WithMessage(ErrorMessages.PracticeRequired);

        // Source validation
        RuleFor(x => x.Source)
            .IsInEnum().WithMessage(ErrorMessages.SourceInvalid);

        // CreatedBy validation
        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required.");
    }
}
