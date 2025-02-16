using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IAntiforgery _antiforgery;

        public TokenController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [HttpGet("GetCsrfToken")]
        public IActionResult GetCsrfToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Console.WriteLine(tokens.RequestToken);
            return Json(new { token = tokens.RequestToken });
        }
    }
}
