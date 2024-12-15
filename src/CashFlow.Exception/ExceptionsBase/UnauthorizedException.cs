using System.Net;

namespace CashFlow.Exception.ExceptionsBase
{
    public class UnauthorizedException : CashFlowException
    {
        public override int StatusCode => (int)HttpStatusCode.Unauthorized;

        public UnauthorizedException() : base(ResourceErrorMessages.LOGIN_UNAUTHORIZED) { }

        public override List<string> GetErrors()
        {
            return new List<string>() { Message };
        }
    }
}
