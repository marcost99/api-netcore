using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Infraestructure;
using CashFlow.Application;
using CashFlow.Infraestructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// If have an exception redirect to class ExceptionFilter
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Method of extension to add dependency injection of DbContext and Repositories
builder.Services.AddInfraestructure(builder.Configuration);

// Method of extension to add dependency injection of business rule
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uses middleware of culture
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await MigrateDatabase();

app.Run();

async Task MigrateDatabase() 
{ 
    //create a scope in the initialization of the application
    await using var scope = app.Services.CreateAsyncScope();
    //makes the migrations
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}