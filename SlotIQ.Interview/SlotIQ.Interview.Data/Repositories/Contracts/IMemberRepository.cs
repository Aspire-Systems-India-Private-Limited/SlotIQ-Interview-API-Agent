using SlotIQ.Interview.Data.Entities;

namespace SlotIQ.Interview.Data.Repositories.Contracts;

public interface IMemberRepository
{
    Task<Member?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default);
    Task<Member?> GetByIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(Guid memberId, DateTime lastLoginDate, CancellationToken cancellationToken = default);
}
