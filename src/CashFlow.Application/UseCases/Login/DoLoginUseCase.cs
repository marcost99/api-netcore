using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUsersReadOnlyRepository _repository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public DoLoginUseCase(IUsersReadOnlyRepository repository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = tokenGenerator;
        }
        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var entity = await _repository.GetByEmail(request.Email);

            if(entity == null)
            {
                throw new UnauthorizedException();
            }

            var passwordMatch = _passwordEncripter.Verifiy(request.Password, entity.Password);

            if (!passwordMatch)
            {
                throw new UnauthorizedException();
            }

            return new ResponseRegisteredUserJson() 
            { 
                Name = entity.Name,
                Token = _accessTokenGenerator.Generate(entity)
            };
        }
    }
}
