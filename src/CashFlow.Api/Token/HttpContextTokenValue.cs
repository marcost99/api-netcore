﻿using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string OnTokenRequest()
        {
            var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return authorization["Bearer ".Length..].Trim();
        }
    }
}
