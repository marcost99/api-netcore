namespace CashFlow.Exception.ExceptionsBase
{
    public abstract class CashFlowException : System.Exception
    {
        public abstract int StatusCode { get; }
        
        protected CashFlowException(string message) : base(message) {}

        public abstract List<string> GetErrors();
    }
}
