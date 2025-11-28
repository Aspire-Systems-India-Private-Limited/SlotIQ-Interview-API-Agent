using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;

namespace SlotIQ.Interview.Data.Repositories.Contracts;

/// <summary>
/// Repository interface for Member entity operations
/// </summary>
public interface IMemberRepository
{
    Task<Result<Member>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Member>>> GetAllAsync();
    Task<PaginatedResult<Member>> GetPagedAsync(int pageNumber, int pageSize);
    Task<Result<Member>> AddAsync(Member entity);
    Task<Result<Member>> UpdateAsync(Member entity);
    Task<Result<string>> DeleteAsync(Guid id);
    Task<bool> UserNameExistsAsync(string userName);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);
}
