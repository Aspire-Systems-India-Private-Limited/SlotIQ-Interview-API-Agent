using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;

namespace SlotIQ.Interview.Data.Repositories;

public class MemberRepository : IMemberRepository
{
    private static readonly List<Member> _members = new()
    {
        new Member
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
        },
        new Member
        {
            MemberID = Guid.Parse("22345678-1234-1234-1234-123456789012"),
            UserName = "jane.smith",
            Firstname = "Jane",
            Lastname = "Smith",
            EmailID = "jane.smith@aspiresys.com",
            PhoneNumber = "9876543210",
            // Password: "Test@123" hashed with BCrypt
            PasswordHash = "$2a$11$OvrWN73xDjXvpG.AUuAMcOoTjgxJF2/LisG/tQrNu16djbo6.nt/S",
            RoleName = MemberRoleEnum.PracticeAdmin,
            PracticeID = Guid.Parse("bbbbbbbb-cccc-dddd-eeee-ffffffffffff"),
            IsActive = true,
            CreatedDate = DateTime.UtcNow.AddDays(-20),
            ModifiedDate = DateTime.UtcNow.AddDays(-20),
            CreatedBy = "System",
            ModifiedBy = "System",
            ModUser = "admin123",
            Source = SourceEnum.Manual
        }
    };

    public Task<Member?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default)
    {
        var member = _members.FirstOrDefault(m =>
            m.UserName.Equals(userNameOrEmail, StringComparison.OrdinalIgnoreCase) ||
            m.EmailID.Equals(userNameOrEmail, StringComparison.OrdinalIgnoreCase));
        
        return Task.FromResult(member);
    }

    public Task<Member?> GetByIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var member = _members.FirstOrDefault(m => m.MemberID == memberId);
        return Task.FromResult(member);
    }

    public Task UpdateLastLoginAsync(Guid memberId, DateTime lastLoginDate, CancellationToken cancellationToken = default)
    {
        var member = _members.FirstOrDefault(m => m.MemberID == memberId);
        if (member != null)
        {
            member.LastLoginDate = lastLoginDate;
            member.ModifiedDate = DateTime.UtcNow;
        }
        return Task.CompletedTask;
    }
}
