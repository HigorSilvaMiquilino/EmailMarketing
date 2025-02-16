using EmailMarketing.Models;
using EmailMarketing.Servicos.Home;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeService _homeService;



        public HomeController(ILogger<HomeController> logger, HomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PhysicalFile("wwwroot/html/index.html", "text/html");
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUserModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _homeService.CreateAsync(model);
                if (token != null)
                {
                    return Ok(new { success = true, message = "Registro realizado com sucesso!", token }); 
                }
                else
                {
                    return BadRequest(new { message = "Erro ao criar o usuário." });
                }

            }

            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }
    }
}
