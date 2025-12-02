using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Queries;
using SlotIQ.Interview.Logic.Queries;

namespace SlotIQ.Interview.UnitTests.Tests.Handlers.Queries;

public class GetMembersPagedQueryHandlerTests
{
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetMembersPagedQueryHandler>> _mockLogger;
    private readonly GetMembersPagedQueryHandler _handler;

    public GetMembersPagedQueryHandlerTests()
    {
        _mockRepository = new Mock<IMemberRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetMembersPagedQueryHandler>>();

        _handler = new GetMembersPagedQueryHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsPagedMembers()
    {
        // Arrange
        var query = new GetMembersPagedQuery(
            PageNumber: 1,
            PageSize: 25,
            SortBy: "CreatedDate",
            SortOrder: "DESC",
            IsActive: true,
            RoleName: MemberRoleEnum.PracticeAdmin,
            PracticeID: Guid.NewGuid());

        var members = CreateTestMembers();
        var memberDtos = CreateTestMemberDtos();

        var paginatedResult = new PaginatedResult<Member>
        {
            Items = members,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 25
        };

        _mockRepository.Setup(r => r.GetMembersPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.SortBy,
                query.SortOrder,
                query.IsActive,
                query.RoleName,
                query.PracticeID))
            .ReturnsAsync(paginatedResult);

        _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(members))
            .Returns(memberDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(25);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_SecondPage_HasPreviousPage()
    {
        // Arrange
        var query = new GetMembersPagedQuery(
            PageNumber: 2,
            PageSize: 25,
            SortBy: "CreatedDate",
            SortOrder: "DESC",
            IsActive: null,
            RoleName: null,
            PracticeID: null);

        var members = CreateTestMembers();
        var memberDtos = CreateTestMemberDtos();

        var paginatedResult = new PaginatedResult<Member>
        {
            Items = members,
            TotalCount = 100,
            PageNumber = 2,
            PageSize = 25
        };

        _mockRepository.Setup(r => r.GetMembersPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.SortBy,
                query.SortOrder,
                query.IsActive,
                query.RoleName,
                query.PracticeID))
            .ReturnsAsync(paginatedResult);

        _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(members))
            .Returns(memberDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithSorting_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var query = new GetMembersPagedQuery(
            PageNumber: 1,
            PageSize: 10,
            SortBy: "UserName",
            SortOrder: "ASC",
            IsActive: true,
            RoleName: null,
            PracticeID: null);

        var paginatedResult = new PaginatedResult<Member>
        {
            Items = new List<Member>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetMembersPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool?>(),
                It.IsAny<MemberRoleEnum?>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync(paginatedResult);

        _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<Member>>()))
            .Returns(new List<MemberDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.GetMembersPagedAsync(
            1,
            10,
            "UserName",
            "ASC",
            true,
            null,
            null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithFilters_CallsRepositoryWithFilters()
    {
        // Arrange
        var practiceId = Guid.NewGuid();
        var query = new GetMembersPagedQuery(
            PageNumber: 1,
            PageSize: 25,
            SortBy: "CreatedDate",
            SortOrder: "DESC",
            IsActive: true,
            RoleName: MemberRoleEnum.TechTeamMember,
            PracticeID: practiceId);

        var paginatedResult = new PaginatedResult<Member>
        {
            Items = new List<Member>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 25
        };

        _mockRepository.Setup(r => r.GetMembersPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool?>(),
                It.IsAny<MemberRoleEnum?>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync(paginatedResult);

        _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<Member>>()))
            .Returns(new List<MemberDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.GetMembersPagedAsync(
            1,
            25,
            "CreatedDate",
            "DESC",
            true,
            MemberRoleEnum.TechTeamMember,
            practiceId), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryException_ReturnsEmptyResult()
    {
        // Arrange
        var query = new GetMembersPagedQuery(
            PageNumber: 1,
            PageSize: 25,
            SortBy: "CreatedDate",
            SortOrder: "DESC",
            IsActive: null,
            RoleName: null,
            PracticeID: null);

        _mockRepository.Setup(r => r.GetMembersPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool?>(),
                It.IsAny<MemberRoleEnum?>(),
                It.IsAny<Guid?>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    private static List<Member> CreateTestMembers()
    {
        return new List<Member>
        {
            new Member
            {
                MemberID = Guid.NewGuid(),
                UserName = "john.doe",
                FirstName = "John",
                LastName = "Doe",
                EmailID = "john.doe@aspiresys.com",
                PhoneNumber = "1234567890",
                RoleID = MemberRoleEnum.PracticeAdmin,
                PracticeID = Guid.NewGuid(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = "system",
                ModifiedBy = "system",
                Source = SourceEnum.Web
            },
            new Member
            {
                MemberID = Guid.NewGuid(),
                UserName = "jane.smith",
                FirstName = "Jane",
                LastName = "Smith",
                EmailID = "jane.smith@aspiresys.com",
                PhoneNumber = "9876543210",
                RoleID = MemberRoleEnum.TechTeamMember,
                PracticeID = Guid.NewGuid(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = "system",
                ModifiedBy = "system",
                Source = SourceEnum.Web
            }
        };
    }

    private static List<MemberDto> CreateTestMemberDtos()
    {
        return new List<MemberDto>
        {
            new MemberDto
            {
                MemberID = Guid.NewGuid(),
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
            },
            new MemberDto
            {
                MemberID = Guid.NewGuid(),
                UserName = "jane.smith",
                Firstname = "Jane",
                Lastname = "Smith",
                EmailID = "jane.smith@aspiresys.com",
                PhoneNumber = "9876543210",
                RoleName = MemberRoleEnum.TechTeamMember,
                PracticeID = Guid.NewGuid(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ModUser = "system",
                Source = SourceEnum.Web
            }
        };
    }
}
