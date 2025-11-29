using Dapper;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;

namespace SlotIQ.Interview.Data.Repositories;

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
            
            var member = await connection.QueryFirstOrDefaultAsync<Member>(query, new { MemberID = id });
            
            if (member == null)
            {
                _logger.LogWarning("Member with ID {MemberID} not found", id);
                return Result<Member>.Failure(ErrorMessages.MemberNotFound);
            }
            
            _logger.LogInformation("Successfully retrieved Member with ID {MemberID}", id);
            return Result<Member>.Success(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Member with ID {MemberID}", id);
            return Result<Member>.Failure(ErrorMessages.DatabaseError);
        }
    }

    public async Task<Result<Member>> GetByUserNameAsync(string userName)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("GetMemberByUserName");
            
            var member = await connection.QueryFirstOrDefaultAsync<Member>(query, new { UserName = userName });
            
            if (member == null)
            {
                return Result<Member>.Failure(ErrorMessages.MemberNotFound);
            }
            
            return Result<Member>.Success(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Member with UserName {UserName}", userName);
            return Result<Member>.Failure(ErrorMessages.DatabaseError);
        }
    }

    public async Task<Result<Member>> GetByEmailAsync(string email)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("GetMemberByEmail");
            
            var member = await connection.QueryFirstOrDefaultAsync<Member>(query, new { EmailID = email });
            
            if (member == null)
            {
                return Result<Member>.Failure(ErrorMessages.MemberNotFound);
            }
            
            return Result<Member>.Success(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Member with Email {Email}", email);
            return Result<Member>.Failure(ErrorMessages.DatabaseError);
        }
    }

    public async Task<Result<Member>> AddAsync(Member entity)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("InsertMember");
            
            var insertedMember = await connection.QuerySingleAsync<Member>(query, entity);
            
            _logger.LogInformation("Successfully created Member with ID {MemberID}", insertedMember.MemberID);
            return Result<Member>.Success(insertedMember);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Member");
            return Result<Member>.Failure(ErrorMessages.DatabaseError);
        }
    }

    public async Task<Result<Member>> UpdateAsync(Member entity)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            // Update implementation would go here
            return Result<Member>.Success(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Member with ID {MemberID}", entity.MemberID);
            return Result<Member>.Failure(ErrorMessages.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            // Delete (soft delete) implementation would go here
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Member with ID {MemberID}", id);
            return Result<bool>.Failure(ErrorMessages.DatabaseError);
        }
    }
}
