using Microsoft.AspNetCore.Mvc;
using EmailMarketing.Data;
using EmailMarketing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromocoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromocao([FromBody] ProducaoModel producaoModel)
        {
            if (producaoModel == null)
            {
                return BadRequest(new { success = false, message = "Dados inválidos." });
            }
            var promocao = new Promocao
            {
                Nome = producaoModel.Nome,
                Descricao = producaoModel.Descricao,
                DataInicio = producaoModel.DataInicio,
                DataTermino = producaoModel.DataTermino,
                Clientes = new List<Cliente>() 
            };

            try
            {
                _context.Promocoes.Add(promocao);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Promoção cadastrada com sucesso!", data = promocao });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPromocoes()
        {
            try
            {
                var promocoes = await _context.Promocoes
                    .Select(p => new
                    {
                        p.Id,
                        p.Nome,
                        p.Descricao,
                        p.DataInicio,
                        p.DataTermino
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = promocoes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}