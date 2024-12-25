using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportsController : ControllerBase
    {
        [HttpGet("pdf")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
