using FluentAssertions;
using Moq;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Validators;

namespace SlotIQ.Interview.UnitTests.Tests.BusinessLogic.Validators;

public class UpdateMemberDtoValidatorTests
{
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly UpdateMemberDtoValidator _validator;

    public UpdateMemberDtoValidatorTests()
    {
        _mockRepository = new Mock<IMemberRepository>();
        _validator = new UpdateMemberDtoValidator(_mockRepository.Object);
    }

    [Fact]
    public async Task Validate_ValidDto_PassesValidation()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_AllFieldsNull_PassesValidation()
    {
        // Arrange - All optional fields are null
        var dto = new UpdateMemberDto
        {
            FirstName = null,
            LastName = null,
            EmailID = null,
            PhoneNumber = null,
            RoleID = null,
            PracticeID = null,
            Source = SourceEnum.API,
            ModifiedBy = "system"
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_FirstNameTooShort_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = "T";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("min 2"));
    }

    [Fact]
    public async Task Validate_FirstNameTooLong_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = new string('a', 51);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("max 50"));
    }

    [Fact]
    public async Task Validate_LastNameTooShort_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = "U";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("min 2"));
    }

    [Fact]
    public async Task Validate_LastNameTooLong_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = new string('a', 51);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("max 50"));
    }

    [Fact]
    public async Task Validate_InvalidEmailFormat_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.EmailID = "invalid-email";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("aspiresys.com domain"));
    }

    [Fact]
    public async Task Validate_EmailNotAspireSysDomain_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.EmailID = "test@gmail.com";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("aspiresys.com domain"));
    }

    [Fact]
    public async Task Validate_InvalidPhoneNumberFormat_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PhoneNumber = "123";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid format"));
    }

    [Fact]
    public async Task Validate_PhoneNumberWithNonDigits_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PhoneNumber = "123-456-789";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid format"));
    }

    [Fact]
    public async Task Validate_InvalidRoleID_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.RoleID = (MemberRoleEnum)999;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid RoleID"));
    }

    [Theory]
    [InlineData("")]
    public async Task Validate_EmptyModifiedBy_FailsValidation(string modifiedBy)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.ModifiedBy = modifiedBy;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("ModifiedBy is required"));
    }

    [Fact]
    public async Task Validate_InvalidSource_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Source = (SourceEnum)999;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid Application SourceID"));
    }

    private UpdateMemberDto CreateValidDto()
    {
        return new UpdateMemberDto
        {
            FirstName = "Test",
            LastName = "User",
            EmailID = "test@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleID = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            Source = SourceEnum.API,
            ModifiedBy = "system"
        };
    }
}
