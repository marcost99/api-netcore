using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.DeleteById
{
    public interface IDeleteByIdExpenseUseCase
    {
        Task Execute(long id);
    }
}
