using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordEncrypter _passwordEncripter;
        private readonly IUsersWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public RegisterUserUseCase(IUsersReadOnlyRepository usersReadOnlyRepository,IMapper mapper, IPasswordEncrypter passwordEncripter, IUsersWriteOnlyRepository repository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
        {
            _usersReadOnlyRepository = usersReadOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var entity = _mapper.Map<Domain.Entities.User>(request);
            entity.Password = _passwordEncripter.Encrypty(request.Password);
            entity.UserIdentifier = Guid.NewGuid();

            await _repository.Add(entity);

            await _unitOfWork.Commit();

            return new ResponseRegisterUserJson()
            {
                Name = entity.Name,
                Token = _accessTokenGenerator.Generate(entity)
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var result = new RegisterUserValidator().Validate(request);

            var emailExist = await _usersReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
