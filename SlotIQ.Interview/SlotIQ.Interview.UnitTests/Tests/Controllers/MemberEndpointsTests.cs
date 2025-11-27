using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.UnitTests.Tests.Controllers;

public class MemberEndpointsTests
{
    private readonly Mock<CreateMemberCommandHandler> _mockHandler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<HttpContext> _mockHttpContext;

    public MemberEndpointsTests()
    {
        _mockHandler = new Mock<CreateMemberCommandHandler>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContext = new Mock<HttpContext>();
    }

    [Fact]
    public void CreateMemberRequest_ValidProperties_ShouldMapCorrectly()
    {
        // Arrange
        var request = new CreateMemberRequest
        {
            UserName = "testuser",
            FirstName = "Test",
            LastName = "User",
            EmailID = "test@aspiresys.com",
            PhoneNumber = "1234567890",
            RoleID = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.NewGuid(),
            Source = SourceEnum.API
        };

        // Assert
        request.UserName.Should().Be("testuser");
        request.FirstName.Should().Be("Test");
        request.LastName.Should().Be("User");
        request.EmailID.Should().Be("test@aspiresys.com");
        request.PhoneNumber.Should().Be("1234567890");
        request.RoleID.Should().Be(MemberRoleEnum.PracticeAdmin);
        request.Source.Should().Be(SourceEnum.API);
    }

    [Fact]
    public void CreateMemberResponse_ValidProperties_ShouldSetCorrectly()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var response = new CreateMemberResponse
        {
            MemberID = memberId,
            SuccessCode = "MEMBER_ONBOARD_SUCCESS",
            SuccessMessage = "User onboarded successfully."
        };

        // Assert
        response.MemberID.Should().Be(memberId);
        response.SuccessCode.Should().Be("MEMBER_ONBOARD_SUCCESS");
        response.SuccessMessage.Should().Be("User onboarded successfully.");
    }

    [Fact]
    public void ApiResponse_Success_ShouldHaveCorrectStructure()
    {
        // Arrange
        var data = new CreateMemberResponse
        {
            MemberID = Guid.NewGuid(),
            SuccessCode = "MEMBER_ONBOARD_SUCCESS",
            SuccessMessage = "User onboarded successfully."
        };

        var response = new ApiResponse<CreateMemberResponse>
        {
            Success = true,
            Data = data
        };

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.MemberID.Should().NotBeEmpty();
        response.ErrorMessage.Should().BeNull();
        response.ErrorCode.Should().BeNull();
    }

    [Fact]
    public void ApiResponse_Failure_ShouldHaveErrorDetails()
    {
        // Arrange
        var response = new ApiResponse<object>
        {
            Success = false,
            ErrorMessage = "Validation failed",
            ErrorCode = "VALIDATION_ERROR"
        };

        // Assert
        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().Be("Validation failed");
        response.ErrorCode.Should().Be("VALIDATION_ERROR");
    }
}
