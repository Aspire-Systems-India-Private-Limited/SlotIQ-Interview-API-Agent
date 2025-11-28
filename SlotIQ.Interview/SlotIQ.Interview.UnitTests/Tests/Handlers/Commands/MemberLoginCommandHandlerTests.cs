using FluentAssertions;
using Moq;
using SlotIQ.Interview.Common.Enums;
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
        var expectedToken = "test-jwt-token";
        
        _memberRepositoryMock
            .Setup(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);
        
        _jwtTokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Member>()))
            .Returns(expectedToken);

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "P@ssw0rd123"
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeTrue();
        token.Should().Be(expectedToken);
        memberDto.Should().NotBeNull();
        memberDto!.UserName.Should().Be("john.doe");
        memberDto.EmailID.Should().Be("john.doe@aspiresys.com");
        memberDto.RoleName.Should().Be(MemberRoleEnum.MasterAdmin);
        errorMessage.Should().BeNull();
        
        _memberRepositoryMock.Verify(x => x.UpdateLastLoginAsync(member.MemberID, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPassword_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var member = CreateTestMember();
        
        _memberRepositoryMock
            .Setup(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "WrongPassword123"
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeFalse();
        token.Should().BeNull();
        memberDto.Should().BeNull();
        errorMessage.Should().Be("Invalid username/email or password.");
        
        _memberRepositoryMock.Verify(x => x.UpdateLastLoginAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentUser_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        _memberRepositoryMock
            .Setup(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "nonexistent@example.com",
            Password = "P@ssw0rd123"
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeFalse();
        token.Should().BeNull();
        memberDto.Should().BeNull();
        errorMessage.Should().Be("User not found.");
        
        _memberRepositoryMock.Verify(x => x.UpdateLastLoginAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyPassword_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = ""
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeFalse();
        token.Should().BeNull();
        memberDto.Should().BeNull();
        errorMessage.Should().Be("Password field is required and cannot be empty.");
        
        _memberRepositoryMock.Verify(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyUserNameOrEmail_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "",
            Password = "P@ssw0rd123"
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeFalse();
        token.Should().BeNull();
        memberDto.Should().BeNull();
        errorMessage.Should().Be("Username or email is required.");
        
        _memberRepositoryMock.Verify(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithInactiveUser_ReturnsFailureWithErrorMessage()
    {
        // Arrange
        var member = CreateTestMember();
        member.IsActive = false;
        
        _memberRepositoryMock
            .Setup(x => x.GetByUserNameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var command = new MemberLoginCommand
        {
            UserNameOrEmail = "john.doe",
            Password = "P@ssw0rd123"
        };

        // Act
        var (success, token, memberDto, errorMessage) = await _handler.HandleAsync(command);

        // Assert
        success.Should().BeFalse();
        token.Should().BeNull();
        memberDto.Should().BeNull();
        errorMessage.Should().Be("User account is not active.");
        
        _memberRepositoryMock.Verify(x => x.UpdateLastLoginAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtTokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Member>()), Times.Never);
    }

    private static Member CreateTestMember()
    {
        return new Member
        {
            MemberID = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            UserName = "john.doe",
            Firstname = "John",
            Lastname = "Doe",
            EmailID = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            // Password: "P@ssw0rd123" hashed with BCrypt
            PasswordHash = "$2a$11$gnugM0ap66XvT22avkSjVeYPKadGm..n7eCRwTPJw24vox0E1fHH.",
            RoleName = MemberRoleEnum.MasterAdmin,
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
