using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase : IGetAllExpenseUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        
        public GetAllExpenseUseCase(IExpensesReadOnlyRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseExpensesJson> Execute()
        {
            var entities = await _repository.GetAll();

            return new ResponseExpensesJson()
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(entities)
            };

            //var response = new ResponseExpensesJson();
            //entities.ForEach(e =>
            //{
            //    response.Expenses.Add
            //    (
            //        new ResponseShortExpenseJson() { Id = e.Id, Title = e.Title, Amount = e.Amount }
            //    );
            //});
            //return response;

            //return new ResponseGetAllExpenseJson()
            //{
            //    Expenses = 
            //        new List<GetAllExpense>()
            //        {
            //            new GetAllExpense() { Id = 1, Title = "Sapato", Amount = 49 },
            //            new GetAllExpense() { Id = 2, Title = "Tenis", Amount = 99 },
            //        }
            //};
        }
    }
}
