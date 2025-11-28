using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.UnitTests.Tests.BusinessLogic.Commands;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _mockLogger;
    private readonly Mock<IValidator<CreateMemberDto>> _mockValidator;
    private readonly CreateMemberCommandHandler _handler;

    public CreateMemberCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IMemberRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CreateMemberCommandHandler>>();
        _mockValidator = new Mock<IValidator<CreateMemberDto>>();

        _handler = new CreateMemberCommandHandler(
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
        var dto = CreateTestDto();
        var command = new CreateMemberCommand(dto);
        var member = CreateTestMember();
        var memberDto = CreateTestMemberDto();

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Member>(dto))
            .Returns(member);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(member));

        _mockMapper.Setup(m => m.Map<MemberDto>(member))
            .Returns(memberDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.MemberID.Should().Be(memberDto.MemberID);
        result.Value.UserName.Should().Be(memberDto.UserName);
    }

    [Fact]
    public async Task Handle_ValidationFailure_ReturnsFailure()
    {
        // Arrange
        var dto = CreateTestDto();
        var command = new CreateMemberCommand(dto);
        
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("UserName", "UserName is required")
        };

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("UserName is required");
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsFailure()
    {
        // Arrange
        var dto = CreateTestDto();
        var command = new CreateMemberCommand(dto);
        var member = CreateTestMember();

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Member>(dto))
            .Returns(member);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Failure("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Database error");
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsRepositoryAdd()
    {
        // Arrange
        var dto = CreateTestDto();
        var command = new CreateMemberCommand(dto);
        var member = CreateTestMember();

        _mockValidator.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Member>(dto))
            .Returns(member);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(member));

        _mockMapper.Setup(m => m.Map<MemberDto>(member))
            .Returns(CreateTestMemberDto());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.Is<Member>(m => 
            m.UserName == dto.UserName &&
            m.EmailID == dto.EmailID)), Times.Once);
    }

    private CreateMemberDto CreateTestDto()
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

    private Member CreateTestMember()
    {
        return new Member
        {
            MemberID = Guid.NewGuid(),
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

    private MemberDto CreateTestMemberDto()
    {
        return new MemberDto
        {
            MemberID = Guid.NewGuid(),
            UserName = "testuser",
            FirstName = "Test",
            LastName = "User",
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
}
