using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Infraestructure;
using CashFlow.Application;
using CashFlow.Infraestructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CashFlow.Infraestructure.Extensions;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Api.Token;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CashFlow.Infraestructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme.\n" +
                      "Enter 'Bearer' [space] and then your token in the text input below.\n" +
                      "Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// If have an exception redirect to class ExceptionFilter
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Method of extension to add dependency injection of DbContext and Repositories
builder.Services.AddInfraestructure(builder.Configuration);

// Method of extension to add dependency injection of business rule
builder.Services.AddApplication();

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHttpContextAccessor();

// Sets the Authorization with JWT
var signingKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey");

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))
    };
});

builder.Services.AddHealthChecks().AddDbContextCheck<CashFlowDbContext>();

var app = builder.Build();

// Sets endpoint to verify the health of api
app.MapHealthChecks("/Health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uses middleware of culture
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (builder.Configuration.IsTestEnvironment() == false)
{
    await MigrateDatabase();
}

app.Run();

async Task MigrateDatabase() 
{ 
    //create a scope in the initialization of the application
    await using var scope = app.Services.CreateAsyncScope();
    //makes the migrations
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}

public partial class Program { }