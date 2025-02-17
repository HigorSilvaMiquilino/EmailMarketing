using EmailMarketing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EmailMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RastreamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RastreamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserIpAddress()
        {
            var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(",").FirstOrDefault()?.Trim();
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        [HttpGet("abertura")]
        public async Task<IActionResult> RegistrarAbertura(string email)
        {
            string userIpAddress = GetUserIpAddress();
            string userAgent = HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

            var logAbertura = new LogAbertura
            {
                DataAbertura = DateTime.Now,
                UserAgent = userAgent,
                IP = userIpAddress,
                Email = email
            };

            try
            {
                _context.LogsAbertura.Add(logAbertura);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Erro ao salvar log de abertura: {exception.Message}");
            }

            return Ok();
        }

        [HttpGet("clique")]
        public async Task<IActionResult> RegistrarClique(string email)
        {
            string userIpAddress = GetUserIpAddress();
            string userAgent = HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

            var logClique = new LogClique
            {
                DataClique = DateTime.Now,
                UserAgent = userAgent,
                IP = userIpAddress,
                Email = email
            };

            try
            {
                _context.LogsClique.Add(logClique);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Erro ao salvar log de clique: {exception.Message}");
            }

            return Redirect("~/html/templates/agradecimento.html");
        }
    }
}
