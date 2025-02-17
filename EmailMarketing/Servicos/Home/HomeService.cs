using EmailMarketing.Data;
using EmailMarketing.Models;
using EmailMarketing.Servicos.Auth;
using EmailMarketing.Servicos.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Servicos.Home
{
    public class HomeService : IHomeService, IEmailHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly EnviarEmail _enviarEmails;
        private readonly AuthService _authService;

        public HomeService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           ApplicationDbContext context,
                           EnviarEmail enviarEmails,
                           AuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _enviarEmails = enviarEmails;
            _authService = authService;
        }

        public async Task<string?> CreateAsync(RegistrarUserModel registerUserModel)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = registerUserModel.Email, 
                NormalizedUserName = _userManager.NormalizeName(registerUserModel.Email),
                
                Nome = registerUserModel.Nome,
                Email = registerUserModel.Email,
                NormalizedEmail = _userManager.NormalizeEmail(registerUserModel.Email)

            };

            var resultado = await _userManager.CreateAsync(applicationUser, registerUserModel.Senha);
            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                var token = _authService.GenerateJwtToken(applicationUser); 
                await _context.SaveChangesAsync();
                return token; 
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return null;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var resultado = await _userManager.DeleteAsync(user);
                if (!resultado.Succeeded)
                {
                    throw new Exception("Error deleting user");
                }
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        /*
        public async Task dispararTodosEmailsAsync(List<string> selectedEmails)
        {
            foreach (var selectedEmail in selectedEmails)
            {
                var data = await _userManager.Users.Where(x => x.Email == selectedEmail).ToListAsync();

                if (data == null || !data.Any())
                {
                    await LogEmailAsync(selectedEmail, EmailStatusEnum.Erro.ToString(), "Usuário não encontrado");
                    throw new Exception("User not found");
                }

                foreach (var user in data)
                {
                    if (user == null || string.IsNullOrEmpty(user.Email))
                    {
                        await LogEmailAsync(selectedEmail, EmailStatusEnum.Erro.ToString(), "Usuário não encontrado");
                        continue;
                    }
                    await _enviarEmails.EnviarEmailslAsync(user.Email, user.Nome);
                    await LogEmailAsync(user.Email, EmailStatusEnum.Sucesso.ToString(), "");
                }
            }
        }
        */
        public async Task<List<UsuarioViewModel>> GetAllAsync()
        {
            var data = await _context.Users
                .OrderBy(x => x.Nome)
                .ToListAsync();

            List<UsuarioViewModel> users = new List<UsuarioViewModel>();

            foreach (var item in data)
            {
                users.Add(new UsuarioViewModel
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Email = item.Email,
                });
            }
            return users;
        }

        public async Task<List<EmailLog>> GetEmailLogsAsync()
        {
            return await _context.EmailLogs.OrderByDescending(log => log.DataEnvio).ToListAsync();
        }

        public async Task LogEmailAsync(string email, string status, string mensagemErro)
        {
            var log = new EmailLog
            {
                Email = email,
                Status = status,
                DataEnvio = DateTime.Now,
                MensagemErro = mensagemErro
            };

            _context.EmailLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public Task LogEmailAsync(string email, string status, string mensagemErro, string stackTrace)
        {
            throw new NotImplementedException();
        }
    }
}
