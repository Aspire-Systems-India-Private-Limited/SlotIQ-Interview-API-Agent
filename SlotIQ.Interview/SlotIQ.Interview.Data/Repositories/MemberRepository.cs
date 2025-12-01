using Dapper;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;

namespace SlotIQ.Interview.Data.Repositories;

/// <summary>
/// Repository implementation for Member entity using Dapper ORM
/// </summary>
public class MemberRepository : IMemberRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ISqlQueryLoader _queryLoader;
    private readonly ILogger<MemberRepository> _logger;

    public MemberRepository(
        IDbConnectionFactory connectionFactory,
        ISqlQueryLoader queryLoader,
        ILogger<MemberRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _queryLoader = queryLoader;
        _logger = logger;
    }

    public async Task<Result<Member>> GetByIdAsync(Guid id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("GetMemberById");

            var entity = await connection.QueryFirstOrDefaultAsync<Member>(query, new { MemberID = id });

            if (entity == null)
            {
                _logger.LogWarning("Member with ID {MemberID} not found", id);
                return Result<Member>.Failure(ErrorMessages.MemberNotFound);
            }

            return Result<Member>.Success(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving member with ID {MemberID}", id);
            return Result<Member>.Failure(ErrorMessages.SystemError);
        }
    }

    public async Task<Result<IEnumerable<Member>>> GetAllAsync()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("GetAllMembers");

            var entities = await connection.QueryAsync<Member>(query);
            return Result<IEnumerable<Member>>.Success(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all members");
            return Result<IEnumerable<Member>>.Failure(ErrorMessages.SystemError);
        }
    }

    public async Task<PaginatedResult<Member>> GetPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("GetPagedMembers");

            var offset = (pageNumber - 1) * pageSize;
            var parameters = new { Offset = offset, PageSize = pageSize };

            var entities = await connection.QueryAsync<Member>(query, parameters);
            
            var countQuery = _queryLoader.LoadQuery("GetMembersCount");
            var totalCount = await connection.ExecuteScalarAsync<int>(countQuery);

            return new PaginatedResult<Member>
            {
                Items = entities,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged members");
            return new PaginatedResult<Member>();
        }
    }

    public async Task<Result<Member>> AddAsync(Member entity)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("InsertMember");

            var parameters = new
            {
                entity.MemberID,
                entity.UserName,
                entity.FirstName,
                entity.LastName,
                entity.Password,
                entity.EmailID,
                entity.PhoneNumber,
                RoleID = (int)entity.RoleID,
                entity.PracticeID,
                entity.IsActive,
                entity.CreatedDate,
                entity.ModifiedDate,
                entity.CreatedBy,
                entity.ModifiedBy,
                Source = (int)entity.Source
            };

            await connection.ExecuteAsync(query, parameters);

            _logger.LogInformation("Member {UserName} created successfully with ID {MemberID}", entity.UserName, entity.MemberID);
            return Result<Member>.Success(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding member {UserName}", entity.UserName);
            return Result<Member>.Failure(ErrorMessages.SystemError);
        }
    }

    public async Task<Result<Member>> UpdateAsync(Member entity)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("UpdateMember");

            var parameters = new
            {
                entity.MemberID,
                entity.UserName,
                entity.FirstName,
                entity.LastName,
                entity.EmailID,
                entity.PhoneNumber,
                RoleID = (int)entity.RoleID,
                entity.PracticeID,
                entity.IsActive,
                entity.ModifiedDate,
                entity.ModifiedBy,
                Source = (int)entity.Source
            };

            var rowsAffected = await connection.ExecuteAsync(query, parameters);

            if (rowsAffected == 0)
            {
                _logger.LogWarning("Member with ID {MemberID} not found for update", entity.MemberID);
                return Result<Member>.Failure(ErrorMessages.MemberNotFound);
            }

            _logger.LogInformation("Member {MemberID} updated successfully", entity.MemberID);
            return Result<Member>.Success(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member {MemberID}", entity.MemberID);
            return Result<Member>.Failure(ErrorMessages.SystemError);
        }
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("DeleteMember");

            var rowsAffected = await connection.ExecuteAsync(query, new { MemberID = id });

            if (rowsAffected == 0)
            {
                _logger.LogWarning("Member with ID {MemberID} not found for deletion", id);
                return Result<string>.Failure(ErrorMessages.MemberNotFound);
            }

            _logger.LogInformation("Member {MemberID} deleted successfully", id);
            return Result<string>.Success("Deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting member {MemberID}", id);
            return Result<string>.Failure(ErrorMessages.SystemError);
        }
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("CheckDuplicateUserName");

            var count = await connection.ExecuteScalarAsync<int>(query, new { UserName = userName });
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate username {UserName}", userName);
            return false;
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("CheckDuplicateEmail");

            var count = await connection.ExecuteScalarAsync<int>(query, new { EmailID = email });
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate email {Email}", email);
            return false;
        }
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("CheckDuplicatePhoneNumber");

            var count = await connection.ExecuteScalarAsync<int>(query, new { PhoneNumber = phoneNumber });
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate phone number {PhoneNumber}", phoneNumber);
            return false;
        }
    }
    
}