using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.UnitTests.Tests.BusinessLogic.Commands;

public class UpdateMemberCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<UpdateMemberCommandHandler>> _mockLogger;
    private readonly Mock<IValidator<UpdateMemberDto>> _mockValidator;
    private readonly UpdateMemberCommandHandler _handler;

    public UpdateMemberCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IMemberRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UpdateMemberCommandHandler>>();
        _mockValidator = new Mock<IValidator<UpdateMemberDto>>();

        _handler = new UpdateMemberCommandHandler(
            _mockUnitOfWork.Object,
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);
        var updatedMember = CreateTestMember(memberId);
        var memberDto = CreateTestMemberDto(memberId);

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.PhoneNumberExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(updatedMember));

        _mockMapper.Setup(m => m.Map<MemberDto>(updatedMember))
            .Returns(memberDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.MemberID.Should().Be(memberId);
    }

    [Fact]
    public async Task Handle_ValidationFailure_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        var command = new UpdateMemberCommand(memberId, dto);
        
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("FirstName", "FirstName must be min 2 and max 50 chars")
        };

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("FirstName must be min 2 and max 50 chars");
    }

    [Fact]
    public async Task Handle_MemberNotFound_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        var command = new UpdateMemberCommand(memberId, dto);

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Failure(ErrorMessages.MemberNotFound));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.MemberNotFound);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        dto.EmailID = "newemail@aspiresys.com";
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);
        existingMember.EmailID = "oldemail@aspiresys.com";

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.EmailExistsAsync("newemail@aspiresys.com"))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.DuplicateEmailAddress);
    }

    [Fact]
    public async Task Handle_DuplicatePhoneNumber_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        dto.PhoneNumber = "9876543210";
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);
        existingMember.PhoneNumber = "1234567890";

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.PhoneNumberExistsAsync("9876543210"))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.DuplicatePhoneNumber);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.PhoneNumberExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Failure("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Database error");
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsRepositoryUpdate()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var dto = CreateTestUpdateDto();
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);
        var updatedMember = CreateTestMember(memberId);

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.PhoneNumberExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(updatedMember));

        _mockMapper.Setup(m => m.Map<MemberDto>(updatedMember))
            .Returns(CreateTestMemberDto(memberId));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<Member>(m => 
            m.MemberID == memberId &&
            m.FirstName == dto.FirstName &&
            m.EmailID == dto.EmailID)), Times.Once);
    }

    [Fact]
    public async Task Handle_OnlyProvidedFieldsAreUpdated()
    {
        // Arrange - Testing partial updates where only FirstName is provided
        // Other fields (LastName, EmailID, PhoneNumber, RoleID, PracticeID) are intentionally null
        // to verify that only explicitly provided fields are updated
        var memberId = Guid.NewGuid();
        var dto = new UpdateMemberDto
        {
            FirstName = "UpdatedFirstName",
            Source = SourceEnum.API,
            ModifiedBy = "system"
        };
        var command = new UpdateMemberCommand(memberId, dto);
        var existingMember = CreateTestMember(memberId);
        var originalLastName = existingMember.LastName;
        var originalEmailID = existingMember.EmailID;

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(existingMember));

        _mockMapper.Setup(m => m.Map<MemberDto>(existingMember))
            .Returns(CreateTestMemberDto(memberId));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<Member>(m => 
            m.FirstName == "UpdatedFirstName" &&
            m.LastName == originalLastName &&
            m.EmailID == originalEmailID)), Times.Once);
    }

    private UpdateMemberDto CreateTestUpdateDto()
    {
        return new UpdateMemberDto
        {
            FirstName = "UpdatedTest",
            LastName = "UpdatedUser",
            EmailID = "updated@aspiresys.com",
            PhoneNumber = "9876543210",
            RoleID = MemberRoleEnum.TechTeamMember,
            PracticeID = Guid.NewGuid(),
            Source = SourceEnum.API,
            ModifiedBy = "system"
        };
    }

    private Member CreateTestMember(Guid memberId)
    {
        return new Member
        {
            MemberID = memberId,
            UserName = "testuser",
            FirstName = "Test",
            LastName = "User",
            Password = "Test@123",
            EmailID = "test@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleID = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = "system",
            ModifiedBy = "system",
            Source = SourceEnum.API
        };
    }

    private MemberDto CreateTestMemberDto(Guid memberId)
    {
        return new MemberDto
        {
            MemberID = memberId,
            UserName = "testuser",
            Firstname = "UpdatedTest",
            Lastname = "UpdatedUser",
            EmailID = "updated@aspiresys.com",
            PhoneNumber = "9876543210",
            RoleName = MemberRoleEnum.TechTeamMember,
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = "system",
            ModifiedBy = "system",
            Source = SourceEnum.API
        };
    }
}
