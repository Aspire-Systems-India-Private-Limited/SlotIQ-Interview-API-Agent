using FluentValidation;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Repositories;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Handlers.Commands;
using SlotIQ.Interview.Logic.Validators;

namespace SlotIQ.Interview.API.Configuration;

/// <summary>
/// Service configuration for dependency injection
/// </summary>
public static class ServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Data layer services
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ISqlQueryLoader, SqlQueryLoader>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IMemberRepository, MemberRepository>();

        // Command Handlers
        services.AddScoped<CreateMemberCommandHandler>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateMemberDtoValidator>();

        // AutoMapper
        services.AddAutoMapper(typeof(Program).Assembly, 
            typeof(SlotIQ.Interview.Logic.Mapping.MemberMappingProfile).Assembly);

        return services;
    }
}
