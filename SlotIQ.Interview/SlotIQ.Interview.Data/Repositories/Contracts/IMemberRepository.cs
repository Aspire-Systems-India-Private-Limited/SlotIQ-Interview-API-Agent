using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;

namespace SlotIQ.Interview.Data.Repositories.Contracts;

public interface IMemberRepository
{
    Task<Result<Member>> GetByIdAsync(Guid id);
    Task<Result<Member>> GetByUserNameAsync(string userName);
    Task<Result<Member>> GetByEmailAsync(string email);
    Task<Result<Member>> AddAsync(Member entity);
    Task<Result<Member>> UpdateAsync(Member entity);
    Task<Result<bool>> DeleteAsync(Guid id);
}
