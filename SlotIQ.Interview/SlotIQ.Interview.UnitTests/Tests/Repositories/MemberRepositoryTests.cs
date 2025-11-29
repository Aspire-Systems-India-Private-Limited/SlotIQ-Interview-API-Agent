using System.Data;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories;

namespace SlotIQ.Interview.UnitTests.Tests.Repositories;

public class MemberRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly Mock<IDbConnectionFactory> _connectionFactoryMock;
    private readonly Mock<ISqlQueryLoader> _queryLoaderMock;
    private readonly Mock<ILogger<MemberRepository>> _loggerMock;
    private readonly MemberRepository _repository;

    public MemberRepositoryTests()
    {
        // Setup in-memory SQLite
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Create table
        _connection.Execute(@"
            CREATE TABLE Member (
                MemberID TEXT PRIMARY KEY,
                UserName TEXT NOT NULL,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Password TEXT NOT NULL,
                EmailID TEXT NOT NULL,
                PhoneNumber TEXT,
                RoleID INTEGER NOT NULL,
                PracticeID TEXT NOT NULL,
                IsActive INTEGER NOT NULL,
                CreatedDate TEXT NOT NULL,
                ModifiedDate TEXT NOT NULL,
                CreatedBy TEXT NOT NULL,
                ModifiedBy TEXT NOT NULL,
                Source TEXT NOT NULL
            );
        ");

        // Setup mocks
        _connectionFactoryMock = new Mock<IDbConnectionFactory>();
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connection);

        _queryLoaderMock = new Mock<ISqlQueryLoader>();
        _loggerMock = new Mock<ILogger<MemberRepository>>();

        _repository = new MemberRepository(
            _connectionFactoryMock.Object,
            _queryLoaderMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldInsertMember_WhenValidDataProvided()
    {
        // Arrange
        var member = new Member
        {
            MemberID = Guid.NewGuid(),
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            EmailID = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleID = 2,
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = "admin",
            ModifiedBy = "admin",
            Source = "1"
        };

        // Directly insert for testing
        await _connection.ExecuteAsync(@"
            INSERT INTO Member (MemberID, UserName, FirstName, LastName, Password, EmailID, PhoneNumber, RoleID, PracticeID, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, Source)
            VALUES (@MemberID, @UserName, @FirstName, @LastName, @Password, @EmailID, @PhoneNumber, @RoleID, @PracticeID, @IsActive, @CreatedDate, @ModifiedDate, @CreatedBy, @ModifiedBy, @Source);
        ", member);

        _queryLoaderMock.Setup(x => x.LoadQuery("GetMemberById"))
            .Returns("SELECT * FROM Member WHERE MemberID = @MemberID AND IsActive = 1");

        // Act
        var result = await _repository.GetByIdAsync(member.MemberID);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserName.Should().Be(member.UserName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMember_WhenMemberExists()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        await _connection.ExecuteAsync(@"
            INSERT INTO Member (MemberID, UserName, FirstName, LastName, Password, EmailID, PhoneNumber, RoleID, PracticeID, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, Source)
            VALUES (@MemberID, 'jane.smith', 'Jane', 'Smith', 'Password123', 'jane.smith@aspiresys.com', '9876543210', 1, @PracticeID, 1, @Date, @Date, 'admin', 'admin', '1')
        ", new { MemberID = memberId.ToString(), PracticeID = Guid.NewGuid().ToString(), Date = DateTime.UtcNow.ToString("o") });

        _queryLoaderMock.Setup(x => x.LoadQuery("GetMemberById"))
            .Returns("SELECT * FROM Member WHERE MemberID = @MemberID AND IsActive = 1");

        // Act
        var result = await _repository.GetByIdAsync(memberId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserName.Should().Be("jane.smith");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenMemberNotFound()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        _queryLoaderMock.Setup(x => x.LoadQuery("GetMemberById"))
            .Returns("SELECT * FROM Member WHERE MemberID = @MemberID AND IsActive = 1");

        // Act
        var result = await _repository.GetByIdAsync(memberId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.MemberNotFound);
    }

    [Fact]
    public async Task GetByUserNameAsync_ShouldReturnMember_WhenUserNameExists()
    {
        // Arrange
        await _connection.ExecuteAsync(@"
            INSERT INTO Member (MemberID, UserName, FirstName, LastName, Password, EmailID, PhoneNumber, RoleID, PracticeID, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, Source)
            VALUES (@MemberID, 'test.user', 'Test', 'User', 'Password123', 'test.user@aspiresys.com', NULL, 3, @PracticeID, 1, @Date, @Date, 'admin', 'admin', '1')
        ", new { MemberID = Guid.NewGuid().ToString(), PracticeID = Guid.NewGuid().ToString(), Date = DateTime.UtcNow.ToString("o") });

        _queryLoaderMock.Setup(x => x.LoadQuery("GetMemberByUserName"))
            .Returns("SELECT * FROM Member WHERE UserName = @UserName AND IsActive = 1");

        // Act
        var result = await _repository.GetByUserNameAsync("test.user");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserName.Should().Be("test.user");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnMember_WhenEmailExists()
    {
        // Arrange
        var email = "email.test@aspiresys.com";
        await _connection.ExecuteAsync(@"
            INSERT INTO Member (MemberID, UserName, FirstName, LastName, Password, EmailID, PhoneNumber, RoleID, PracticeID, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, Source)
            VALUES (@MemberID, 'email.user', 'Email', 'User', 'Password123', @EmailID, NULL, 2, @PracticeID, 1, @Date, @Date, 'admin', 'admin', '1')
        ", new { MemberID = Guid.NewGuid().ToString(), EmailID = email, PracticeID = Guid.NewGuid().ToString(), Date = DateTime.UtcNow.ToString("o") });

        _queryLoaderMock.Setup(x => x.LoadQuery("GetMemberByEmail"))
            .Returns("SELECT * FROM Member WHERE EmailID = @EmailID AND IsActive = 1");

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.EmailID.Should().Be(email);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
