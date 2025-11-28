using FluentAssertions;
using Moq;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Validators;

namespace SlotIQ.Interview.UnitTests.Tests.BusinessLogic.Validators;

public class CreateMemberDtoValidatorTests
{
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly CreateMemberDtoValidator _validator;

    public CreateMemberDtoValidatorTests()
    {
        _mockRepository = new Mock<IMemberRepository>();
        _validator = new CreateMemberDtoValidator(_mockRepository.Object);
    }

    [Fact]
    public async Task Validate_ValidDto_PassesValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        _mockRepository.Setup(r => r.UserNameExistsAsync(dto.UserName)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.EmailExistsAsync(dto.EmailID)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.PhoneNumberExistsAsync(dto.PhoneNumber!)).ReturnsAsync(false);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyUserName_FailsValidation(string userName)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.UserName = userName!;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("UserName is required"));
    }

    [Fact]
    public async Task Validate_UserNameTooShort_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.UserName = "test";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("min 5 chars"));
    }

    [Fact]
    public async Task Validate_UserNameTooLong_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.UserName = new string('a', 101);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("max 100 chars"));
    }

    [Fact]
    public async Task Validate_InvalidUserNameFormat_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.UserName = "test user!";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Active Directory format"));
    }

    [Fact]
    public async Task Validate_DuplicateUserName_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        _mockRepository.Setup(r => r.UserNameExistsAsync(dto.UserName)).ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("UserName already exists"));
    }

    [Fact]
    public async Task Validate_EmptyFirstName_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = "";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("First name is required"));
    }

    [Fact]
    public async Task Validate_EmptyLastName_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = "";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Last name is required"));
    }

    [Fact]
    public async Task Validate_EmptyEmail_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.EmailID = "";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("EmailAddress is required"));
    }

    [Fact]
    public async Task Validate_InvalidEmailDomain_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.EmailID = "test@gmail.com";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("aspiresys.com"));
    }

    [Fact]
    public async Task Validate_DuplicateEmail_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        _mockRepository.Setup(r => r.UserNameExistsAsync(dto.UserName)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.EmailExistsAsync(dto.EmailID)).ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("EmailAddress already exists"));
    }

    [Fact]
    public async Task Validate_InvalidPhoneNumber_FailsValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PhoneNumber = "12345";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid format"));
    }

    [Fact]
    public async Task Validate_EmptyPhoneNumber_PassesValidation()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PhoneNumber = null;
        _mockRepository.Setup(r => r.UserNameExistsAsync(dto.UserName)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.EmailExistsAsync(dto.EmailID)).ReturnsAsync(false);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    private CreateMemberDto CreateValidDto()
    {
        return new CreateMemberDto
        {
            UserName = "testuser",
            FirstName = "Test",
            LastName = "User",
            EmailID = "test@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleID = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            Source = SourceEnum.API,
            CreatedBy = "system"
        };
    }
}
