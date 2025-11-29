using System.Text.Json.Serialization;
using FluentValidation;
using SlotIQ.Interview.API.Endpoints;
using SlotIQ.Interview.API.Mapping;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Repositories;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;
using SlotIQ.Interview.Logic.Mapping;
using SlotIQ.Interview.Logic.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Configure JSON options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Infrastructure services
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<ISqlQueryLoader, SqlQueryLoader>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MemberMappingProfile), typeof(MemberApiMappingProfile));

// Validators
builder.Services.AddScoped<IValidator<CreateMemberDto>, CreateMemberDtoValidator>();

// Command Handlers
builder.Services.AddScoped<CreateMemberCommandHandler>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("ApiCorsPolicy");

// Map endpoints
app.MapMemberEndpoints();

app.Run();

