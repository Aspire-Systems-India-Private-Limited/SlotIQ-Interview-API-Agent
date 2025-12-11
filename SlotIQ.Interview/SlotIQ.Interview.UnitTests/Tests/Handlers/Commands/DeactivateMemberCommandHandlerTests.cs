using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Handlers.Commands;
using Xunit;

namespace SlotIQ.Interview.UnitTests.Tests.Handlers.Commands;

public class DeactivateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<ILogger<DeactivateMemberCommandHandler>> _loggerMock;
    private readonly DeactivateMemberCommandHandler _handler;

    public DeactivateMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new Mock<IMemberRepository>();
        _loggerMock = new Mock<ILogger<DeactivateMemberCommandHandler>>();
        _handler = new DeactivateMemberCommandHandler(_memberRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidMemberID_ReturnsSuccess()
    {
        // Arrange
        var memberID = Guid.NewGuid();
        var modifiedBy = "admin@aspiresys.com";
        var source = SourceEnum.Web;
        
        _memberRepositoryMock
            .Setup(x => x.DeactivateMemberAsync(memberID, modifiedBy, source))
            .ReturnsAsync(Result<string>.Success(memberID.ToString()));

        var command = new DeactivateMemberCommand(memberID, modifiedBy, source);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(memberID.ToString());
        
        _memberRepositoryMock.Verify(
            x => x.DeactivateMemberAsync(memberID, modifiedBy, source),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MemberNotFound_ReturnsFailure()
    {
        // Arrange
        var memberID = Guid.NewGuid();
        var modifiedBy = "admin@aspiresys.com";
        var source = SourceEnum.Web;
        
        _memberRepositoryMock
            .Setup(x => x.DeactivateMemberAsync(memberID, modifiedBy, source))
            .ReturnsAsync(Result<string>.Failure(ErrorMessages.MemberNotFoundOrInactive));

        var command = new DeactivateMemberCommand(memberID, modifiedBy, source);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.MemberNotFoundOrInactive);
        
        _memberRepositoryMock.Verify(
            x => x.DeactivateMemberAsync(memberID, modifiedBy, source),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MemberAlreadyInactive_ReturnsFailure()
    {
        // Arrange
        var memberID = Guid.NewGuid();
        var modifiedBy = "admin@aspiresys.com";
        var source = SourceEnum.Web;
        
        _memberRepositoryMock
            .Setup(x => x.DeactivateMemberAsync(memberID, modifiedBy, source))
            .ReturnsAsync(Result<string>.Failure(ErrorMessages.MemberNotFoundOrInactive));

        var command = new DeactivateMemberCommand(memberID, modifiedBy, source);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.MemberNotFoundOrInactive);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsSystemError()
    {
        // Arrange
        var memberID = Guid.NewGuid();
        var modifiedBy = "admin@aspiresys.com";
        var source = SourceEnum.Web;
        
        _memberRepositoryMock
            .Setup(x => x.DeactivateMemberAsync(memberID, modifiedBy, source))
            .ThrowsAsync(new Exception("Database error"));

        var command = new DeactivateMemberCommand(memberID, modifiedBy, source);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorMessages.SystemError);
    }

    [Fact]
    public async Task Handle_WithDifferentSources_DeactivatesSuccessfully()
    {
        // Arrange
        var memberID = Guid.NewGuid();
        var modifiedBy = "admin@aspiresys.com";
        
        var sources = new[] { SourceEnum.Web, SourceEnum.Mobile, SourceEnum.API };

        foreach (var source in sources)
        {
            _memberRepositoryMock
                .Setup(x => x.DeactivateMemberAsync(memberID, modifiedBy, source))
                .ReturnsAsync(Result<string>.Success(memberID.ToString()));

            var command = new DeactivateMemberCommand(memberID, modifiedBy, source);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(memberID.ToString());
        }
    }
}
