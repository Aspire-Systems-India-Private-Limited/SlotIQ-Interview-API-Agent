using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.UnitTests.Tests.Handlers;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _loggerMock;
    private readonly Mock<IValidator<CreateMemberDto>> _validatorMock;
    private readonly CreateMemberCommandHandler _handler;

    public CreateMemberCommandHandlerTests()
    {
        _repositoryMock = new Mock<IMemberRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateMemberCommandHandler>>();
        _validatorMock = new Mock<IValidator<CreateMemberDto>>();

        _handler = new CreateMemberCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateMember_WhenValidDataProvided()
    {
        // Arrange
        var dto = new CreateMemberDto
        {
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            RoleName = MemberRole.PracticeAdmin,
            EmailAddress = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            UpdatedBy = "admin",
            Source = Source.Web
        };

        var command = new CreateMemberCommand(dto);

        var entity = new Member
        {
            MemberID = Guid.NewGuid(),
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Password = dto.Password,
            EmailID = dto.EmailAddress,
            PhoneNumber = dto.PhoneNumber,
            RoleID = (int)dto.RoleName,
            PracticeID = dto.PracticeID,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = dto.UpdatedBy,
            ModifiedBy = dto.UpdatedBy,
            Source = ((int)dto.Source).ToString()
        };

        var memberDto = new MemberDto
        {
            MemberID = entity.MemberID,
            UserName = entity.UserName,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            EmailID = entity.EmailID,
            PhoneNumber = entity.PhoneNumber,
            RoleName = dto.RoleName,
            PracticeID = entity.PracticeID,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            ModUser = entity.ModifiedBy,
            Source = dto.Source
        };

        _validatorMock.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(x => x.Map<Member>(dto)).Returns(entity);
        _mapperMock.Setup(x => x.Map<MemberDto>(entity)).Returns(memberDto);

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Success(entity));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserName.Should().Be("john.doe");
        result.Value.FirstName.Should().Be("John");
        result.Value.EmailID.Should().Be("john.doe@aspiresys.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var dto = new CreateMemberDto
        {
            UserName = "ab", // Too short
            FirstName = "John",
            LastName = "Doe",
            Password = "pass", // Too short
            RoleName = MemberRole.PracticeAdmin,
            EmailAddress = "invalid-email",
            PhoneNumber = "123",
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            UpdatedBy = "admin",
            Source = Source.Web
        };

        var command = new CreateMemberCommand(dto);

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("UserName", ErrorMessages.MinLength("UserName", 5)),
            new ValidationFailure("Password", ErrorMessages.MinLength("Password", 8)),
            new ValidationFailure("EmailAddress", ErrorMessages.InvalidEmail)
        };

        _validatorMock.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        result.Error.Should().Contain("UserName");
        result.Error.Should().Contain("Password");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var dto = new CreateMemberDto
        {
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            RoleName = MemberRole.PracticeAdmin,
            EmailAddress = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            UpdatedBy = "admin",
            Source = Source.Web
        };

        var command = new CreateMemberCommand(dto);
        var entity = new Member();

        _validatorMock.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(x => x.Map<Member>(dto)).Returns(entity);

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync(Result<Member>.Failure(ErrorMessages.DatabaseError));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.DatabaseError);
    }

    [Fact]
    public async Task Handle_ShouldSetAuditFields_WhenCreatingMember()
    {
        // Arrange
        var dto = new CreateMemberDto
        {
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            RoleName = MemberRole.PracticeAdmin,
            EmailAddress = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            UpdatedBy = "admin",
            Source = Source.Web
        };

        var command = new CreateMemberCommand(dto);
        Member? capturedEntity = null;

        _validatorMock.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(x => x.Map<Member>(dto)).Returns(new Member());

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Member>()))
            .Callback<Member>(e => capturedEntity = e)
            .ReturnsAsync((Member m) => Result<Member>.Success(m));

        _mapperMock.Setup(x => x.Map<MemberDto>(It.IsAny<Member>()))
            .Returns(new MemberDto());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.MemberID.Should().NotBe(Guid.Empty);
        capturedEntity.CreatedBy.Should().Be("admin");
        capturedEntity.ModifiedBy.Should().Be("admin");
        capturedEntity.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        capturedEntity.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
