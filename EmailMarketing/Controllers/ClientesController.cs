using EmailMarketing.Data;
using EmailMarketing.Servicos.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {
        private readonly ClientesService _clientesService;

        public ClientesController(ClientesService clientesService)
        {
            _clientesService = clientesService;
        }
        [HttpGet("PorArquivoNome")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesPorArquivoNome(string arquivoNome)
        {
            Console.WriteLine(arquivoNome);
            var clientes = await _clientesService.GetClientesPorArquivoNome(arquivoNome);

            if (clientes == null || !clientes.Any())
            {
                return NotFound();
            }
            return Ok(new { success = true, message = "Clientes encontrados com sucesso", cliente = clientes });
        }

        [HttpGet("ArquivoNome")]
        public async Task<ActionResult<IEnumerable<string>>> GetTodosrquivoNome()
        {
            var clientes = await _clientesService.GetTodosArquivoNome();

            if (clientes == null || !clientes.Any())
            {
                return NotFound();
            }

            return Ok(new { success = true, message = "Arquivos encontrados com sucesso", cliente = clientes });
        }
    }
}
