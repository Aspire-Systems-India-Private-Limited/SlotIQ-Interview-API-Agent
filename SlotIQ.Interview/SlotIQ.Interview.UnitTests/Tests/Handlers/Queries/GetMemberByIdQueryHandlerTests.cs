using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Queries;
using SlotIQ.Interview.Logic.Queries;

namespace SlotIQ.Interview.UnitTests.Tests.Handlers.Queries;

public class GetMemberByIdQueryHandlerTests
{
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetMemberByIdQueryHandler>> _mockLogger;
    private readonly GetMemberByIdQueryHandler _handler;

    public GetMemberByIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IMemberRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetMemberByIdQueryHandler>>();

        _handler = new GetMemberByIdQueryHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidMemberId_ReturnsMemberDto()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        var member = CreateTestMember(memberId);
        var memberDto = CreateTestMemberDto(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(member));

        _mockMapper.Setup(m => m.Map<MemberDto>(member))
            .Returns(memberDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.MemberID.Should().Be(memberId);
        result.Value.UserName.Should().Be("john.doe");
        result.Value.Firstname.Should().Be("John");
        result.Value.Lastname.Should().Be("Doe");
        result.Value.EmailID.Should().Be("john.doe@aspiresys.com");
    }

    [Fact]
    public async Task Handle_MemberNotFound_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Failure(ErrorMessages.MemberNotFound));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.MemberNotFound);
    }

    [Fact]
    public async Task Handle_ValidMemberId_CallsRepositoryOnce()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        var member = CreateTestMember(memberId);
        var memberDto = CreateTestMemberDto(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(member));

        _mockMapper.Setup(m => m.Map<MemberDto>(member))
            .Returns(memberDto);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidMemberId_CallsMapperOnce()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        var member = CreateTestMember(memberId);
        var memberDto = CreateTestMemberDto(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Success(member));

        _mockMapper.Setup(m => m.Map<MemberDto>(member))
            .Returns(memberDto);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockMapper.Verify(m => m.Map<MemberDto>(member), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryException_ReturnsFailure()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("error occurred");
    }

    [Fact]
    public async Task Handle_MemberNotFound_DoesNotCallMapper()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var query = new GetMemberByIdQuery(memberId);

        _mockRepository.Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(Result<Member>.Failure(ErrorMessages.MemberNotFound));

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockMapper.Verify(m => m.Map<MemberDto>(It.IsAny<Member>()), Times.Never);
    }

    private static Member CreateTestMember(Guid memberId)
    {
        return new Member
        {
            MemberID = memberId,
            UserName = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            EmailID = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            Password = "hashedpassword",
            RoleID = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = "system",
            ModifiedBy = "system",
            Source = SourceEnum.Web
        };
    }

    private static MemberDto CreateTestMemberDto(Guid memberId)
    {
        return new MemberDto
        {
            MemberID = memberId,
            UserName = "john.doe",
            Firstname = "John",
            Lastname = "Doe",
            EmailID = "john.doe@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleName = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            ModUser = "system",
            Source = SourceEnum.Web
        };
    }
}
