using Microsoft.AspNetCore.Mvc;
using EmailMarketing.Data;
using EmailMarketing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmailMarketing.Controllers
{
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
    }
}