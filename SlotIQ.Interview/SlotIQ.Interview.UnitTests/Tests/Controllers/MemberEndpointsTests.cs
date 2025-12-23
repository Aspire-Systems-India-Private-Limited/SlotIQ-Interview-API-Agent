using FluentAssertions;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;

namespace SlotIQ.Interview.UnitTests.Tests.Controllers;

/// <summary>
/// Tests for API models and response structures
/// </summary>
public class MemberEndpointsTests
{

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
    public void UpdateMemberRequest_ValidProperties_ShouldMapCorrectly()
    {
        // Arrange
        var request = new UpdateMemberRequest
        {
            FirstName = "UpdatedTest",
            LastName = "UpdatedUser",
            EmailAddress = "updated@aspiresys.com",
            PhoneNumber = "9876543210",
            RoleName = MemberRoleEnum.TechTeamMember,
            PracticeID = Guid.NewGuid(),
            Source = SourceEnum.API,
            UpdatedBy = "system"
        };

        // Assert
        request.FirstName.Should().Be("UpdatedTest");
        request.LastName.Should().Be("UpdatedUser");
        request.EmailAddress.Should().Be("updated@aspiresys.com");
        request.PhoneNumber.Should().Be("9876543210");
        request.RoleName.Should().Be(MemberRoleEnum.TechTeamMember);
        request.Source.Should().Be(SourceEnum.API);
        request.UpdatedBy.Should().Be("system");
    }

    [Fact]
    public void UpdateMemberRequest_OptionalFields_CanBeNull()
    {
        // Arrange
        var request = new UpdateMemberRequest
        {
            FirstName = null,
            LastName = null,
            EmailAddress = null,
            PhoneNumber = null,
            RoleName = null,
            PracticeID = null,
            Source = SourceEnum.API,
            UpdatedBy = "system"
        };

        // Assert
        request.FirstName.Should().BeNull();
        request.LastName.Should().BeNull();
        request.EmailAddress.Should().BeNull();
        request.PhoneNumber.Should().BeNull();
        request.RoleName.Should().BeNull();
        request.PracticeID.Should().BeNull();
        request.UpdatedBy.Should().Be("system");
    }

    [Fact]
    public void GetMembersResponse_ValidProperties_ShouldSetCorrectly()
    {
        // Arrange
        var response = new GetMembersResponse
        {
            SuccessCode = "MEMBER_LIST_SUCCESS",
            SuccessMessage = "Members retrieved successfully.",
            TotalCount = 100,
            PageNumber = 1,
            PageSize = 25,
            HasNext = true,
            HasPrevious = false,
            Items = new List<Logic.Dtos.MemberDto>
            {
                new Logic.Dtos.MemberDto
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
                    CreatedBy = "system",
                    ModifiedBy = "system",
                    Source = SourceEnum.Web
                }
            }
        };

        // Assert
        response.SuccessCode.Should().Be("MEMBER_LIST_SUCCESS");
        response.SuccessMessage.Should().Be("Members retrieved successfully.");
        response.TotalCount.Should().Be(100);
        response.PageNumber.Should().Be(1);
        response.PageSize.Should().Be(25);
        response.HasNext.Should().BeTrue();
        response.HasPrevious.Should().BeFalse();
        response.Items.Should().HaveCount(1);
        response.Items.First().UserName.Should().Be("john.doe");
    }

    [Fact]
    public void UpdateMemberResponse_ValidProperties_ShouldSetCorrectly()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var response = new UpdateMemberResponse
        {
            MemberID = memberId,
            SuccessCode = "MEMBER_UPDATE_SUCCESS",
            SuccessMessage = "Member details updated successfully."
        };

        // Assert
        response.MemberID.Should().Be(memberId);
        response.SuccessCode.Should().Be("MEMBER_UPDATE_SUCCESS");
        response.SuccessMessage.Should().Be("Member details updated successfully.");
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
