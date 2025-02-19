
using EmailMarketing.Data;
using EmailMarketing.Servicos.Auth;
using EmailMarketing.Servicos.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Servicos.Recuperacao
{
    public class RecuperacaoService : IRecuperacaoService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RecuperacaoService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> getEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }
    }
}
