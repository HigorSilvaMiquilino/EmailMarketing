using EmailMarketing.Models;
using EmailMarketing.Servicos.Email;
using EmailMarketing.Servicos.Home;
using EmailMarketing.Servicos.Recuperacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecuperacaoController : Controller
    {
        private readonly HomeService _homeService;
        private readonly RecuperacaoService _recuperacaoService;
        private readonly EnviarEmail _enviarEmails;


        public RecuperacaoController(HomeService homeService, RecuperacaoService recuperacao, EnviarEmail enviarEmails)
        {
            _homeService = homeService;
            _recuperacaoService = recuperacao;
            _enviarEmails = enviarEmails;
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetEmail(string email)
        {
            try
            {
                var emailExists = await _recuperacaoService.getEmail(email);

                if (!emailExists)
                {
                    return NotFound(new { success = false, message = "E-mail não encontrado." });
                }

                return Ok(new { success = true , message = "E-mail encontrado, iremos encaminha o e-mail de recuperação de senha." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("disparo")]
        public async Task<IActionResult> recuperacaoDisparo(string email)
        {
            try
            {
                var user = await _homeService.GetUsuarioEmailAsync(email);
                await _enviarEmails.EnviarEmailRecuperacao(user);
                return Ok(new { success = true, message = "E-mail enviado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("recuperar")]
        public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaModel model)
        {
            try
            {
                var user = await _homeService.GetUsuarioEmailAsync(model.Email);
                var result = await _homeService.ResetSenhaAsync(user, model.Senha);
                if (result)
                {
                    return Ok(new { success = true, message = "Senha alterada com sucesso, você pode fazer o login agora" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Erro ao alterar a senha." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
