using FluentAssertions;
using Moq;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Handlers.Commands;
using SlotIQ.Interview.Logic.Services;
using Xunit;

namespace SlotIQ.Interview.UnitTests.Tests.Handlers.Commands;

public class MemberLoginCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly MemberLoginCommandHandler _handler;

    public MemberLoginCommandHandlerTests()
    {
        _memberRepositoryMock = new Mock<IMemberRepository>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _handler = new MemberLoginCommandHandler(_memberRepositoryMock.Object, _jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidCredentials_ReturnsSuccessWithToken()
    {
        // Arrange
        var member = CreateTestMember();
        var members = new List<Member> { member };
        var expectedToken = "test-jwt-token";
        
        _memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(Result<IEnumerable<Member>>.Success(members));
        
        _jwtTokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Member>()))
            .Returns(expectedToken);

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "P@ssw0rd123"
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Token.Should().Be(expectedToken);
        result.Value.Member.Should().NotBeNull();
        result.Value.Member.UserName.Should().Be("john.doe");
        result.Value.Member.EmailID.Should().Be("john.doe@aspiresys.com");
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPassword_ReturnsFailure()
    {
        // Arrange
        var member = CreateTestMember();
        var members = new List<Member> { member };
        
        _memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(Result<IEnumerable<Member>>.Success(members));

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "WrongPassword123"
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Invalid");
        
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentUser_ReturnsFailure()
    {
        // Arrange
        var members = new List<Member>();
        
        _memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(Result<IEnumerable<Member>>.Success(members));

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "nonexistent@example.com",
            Password = "P@ssw0rd123"
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
        
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyPassword_ReturnsFailure()
    {
        // Arrange
        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = ""
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Password");
        
        _memberRepositoryMock.Verify(x => x.GetAllAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyUserNameOrEmail_ReturnsFailure()
    {
        // Arrange
        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "",
            Password = "P@ssw0rd123"
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("UserName is required");
        
        _memberRepositoryMock.Verify(x => x.GetAllAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithInactiveUser_ReturnsFailure()
    {
        // Arrange
        var member = CreateTestMember();
        member.IsActive = false;
        var members = new List<Member> { member };
        
        _memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(Result<IEnumerable<Member>>.Success(members));

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "P@ssw0rd123"
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not active");
        
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    private static Member CreateTestMember()
    {
        return new Member
        {
            MemberID = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            EmailID = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            // Password: "P@ssw0rd123" hashed with BCrypt
            Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123"),
            RoleID = MemberRoleEnum.MasterAdmin,
            PracticeID = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            IsActive = true,
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            ModifiedDate = DateTime.UtcNow.AddDays(-30),
            CreatedBy = "System",
            ModifiedBy = "System",
            ModUser = "admin123",
            Source = SourceEnum.Manual
        };
    }
}
