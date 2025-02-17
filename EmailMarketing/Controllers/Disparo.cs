using Microsoft.AspNetCore.Mvc;
using EmailMarketing.Data;
using EmailMarketing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using EmailMarketing.Servicos.Email;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DisparoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EnviarEmail _enviarEmails;

        public DisparoController(ApplicationDbContext context, EnviarEmail enviarEmail)
        {
            _context = context;
            _enviarEmails = enviarEmail;

        }

        [HttpPost]
        public async Task<IActionResult> DispararEmails([FromForm] DisparoModel disparoModel)
        {
            if (disparoModel == null)
            {
                return BadRequest(new { success = false, message = "Dados inválidos." });
            }

            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.PromocaoId == disparoModel.PromocaoId)
                    .ToListAsync();

                if (clientes.Count == 0)
                {
                    return BadRequest(new { success = false, message = "Nenhum cliente encontrado para a promoção selecionada." });
                }

                var promocao = await _context.Promocoes.FindAsync(disparoModel.PromocaoId);

                string imagemUrl = null;
                if (disparoModel.ImagemPromocao != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + disparoModel.ImagemPromocao.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await disparoModel.ImagemPromocao.CopyToAsync(stream);
                    }

                    imagemUrl = $"/uploads/{uniqueFileName}";
                    Console.WriteLine($"Imagem em: {imagemUrl}");
                }
                
                foreach (var cliente in clientes)
                {
                    await _enviarEmails.EnviarEmailslAsync(cliente.Email, cliente.Nome, disparoModel, imagemUrl, promocao);
                    await _enviarEmails.LogEmailAsync(cliente.Email, EmailStatusEnum.Sucesso.ToString(), "E-mail enviado com sucesso.");
                    Console.WriteLine($"Enviando e-mail para {cliente.Email} com assunto: {disparoModel.Assunto}");
                }

                return Ok(new { success = true, message = "E-mails disparados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}