using EmailMarketing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("envio")]
        public async Task<IActionResult> GetLogsEnvio()
        {
            var logs = await _context.LogsEnvio
                .GroupBy(l => l.DataEnvio.Month) 
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(g => g.Mes)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("abertura")]
        public async Task<IActionResult> GetLogsAbertura()
        {
            var logs = await _context.LogsAbertura
                .GroupBy(l => l.DataAbertura.Month) 
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(g => g.Mes)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("clique")]
        public async Task<IActionResult> GetLogsClique()
        {
            var logs = await _context.LogsClique
                .GroupBy(l => l.DataClique.Month) 
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(g => g.Mes)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("erro")]
        public async Task<IActionResult> GetLogsErro()
        {
            var logs = await _context.LogsErro
                .GroupBy(l => l.DataErro.Month) 
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(g => g.Mes)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
