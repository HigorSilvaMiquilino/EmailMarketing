using Microsoft.AspNetCore.Mvc;
using EmailMarketing.Data;
using EmailMarketing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using EmailMarketing.Servicos.Email;
using System;

namespace EmailMarketing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DisparoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EnviarEmail _enviarEmails;
        private string Email { get; set; }
        private string imagemUrl;

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

                Email = clientes[0].Email;

                var promocao = await _context.Promocoes.FindAsync(disparoModel.PromocaoId);

                if (promocao == null)
                {
                    return BadRequest(new { success = false, message = "Promoção não encontrada." });
                }


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

                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
                    imagemUrl = $"{baseUrl}/uploads/{uniqueFileName}";
                    Console.WriteLine($"Imagem em: {imagemUrl}");
                }

                
                foreach (var cliente in clientes)
                {
                    await _enviarEmails.EnviarEmailslAsync(cliente.Email, cliente.Nome, disparoModel, imagemUrl, promocao);
                    var logEnvio = new LogEnvio
                    {
                        Email = cliente.Email,
                        Assunto = disparoModel.Assunto,
                        DataEnvio = DateTime.Now,
                        PromocaoId = disparoModel.PromocaoId,
                        Status = EmailStatusEnum.Sucesso.ToString()
                    };
                    _context.LogsEnvio.Add(logEnvio);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { success = true, message = "E-mails disparados com sucesso!" });
            }
            catch (Exception ex)
            {
                var logErro = new LogErro
                {
                    DataErro = DateTime.Now,
                    MensagemErro = ex.Message,
                    StackTrace = ex.StackTrace,
                    Status = EmailStatusEnum.Erro.ToString(),
                    Email = Email
                };

                try
                {
                    _context.LogsErro.Add(logErro);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Erro ao salvar log de erro: {exception.Message}");
                }

                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}