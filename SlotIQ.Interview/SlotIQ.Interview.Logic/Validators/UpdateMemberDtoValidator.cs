using FluentValidation;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Validators;

/// <summary>
/// Validator for UpdateMemberDto
/// </summary>
public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public UpdateMemberDtoValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;

        // FirstName validation (optional)
        RuleFor(x => x.FirstName)
            .Length(2, 50).WithMessage(ErrorMessages.FirstNameLength)
            .When(x => !string.IsNullOrEmpty(x.FirstName));

        // LastName validation (optional)
        RuleFor(x => x.LastName)
            .Length(2, 50).WithMessage(ErrorMessages.LastNameLength)
            .When(x => !string.IsNullOrEmpty(x.LastName));

        // EmailID validation (optional)
        RuleFor(x => x.EmailID)
            .EmailAddress().WithMessage(ErrorMessages.EmailAddressDomain)
            .Must(email => email!.EndsWith("@aspiresys.com", StringComparison.OrdinalIgnoreCase))
            .WithMessage(ErrorMessages.EmailAddressDomain)
            .When(x => !string.IsNullOrEmpty(x.EmailID));

        // PhoneNumber validation (optional)
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[0-9]{10}$").WithMessage(ErrorMessages.PhoneNumberFormat)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // RoleID validation (optional)
        RuleFor(x => x.RoleID)
            .IsInEnum().WithMessage(ErrorMessages.RoleInvalid)
            .When(x => x.RoleID.HasValue);

        // Source validation (required)
        RuleFor(x => x.Source)
            .IsInEnum().WithMessage(ErrorMessages.SourceInvalid);

        // ModifiedBy validation (required)
        RuleFor(x => x.ModifiedBy)
            .NotEmpty().WithMessage("ModifiedBy is required.");
    }
}
