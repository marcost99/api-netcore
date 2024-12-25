using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infraestructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private CashFlow.Domain.Entities.User _user;
        private string _password;
        public string GetEmail() => _user.Email;
        public string GetName() => _user.Name;
        public string GetPassword() => _password;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                    services.AddDbContext<CashFlowDbContext>(config => 
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();

                    StartDatabase(dbContext, passwordEncripter);
                });
        }

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncrypter passwordEncripter)
        {
            _user = UserBuilder.Build();
            _password = _user.Password;
            _user.Password = passwordEncripter.Encrypty(_user.Password);

            dbContext.User.Add(_user);

            dbContext.SaveChanges();
        }
    }
}
