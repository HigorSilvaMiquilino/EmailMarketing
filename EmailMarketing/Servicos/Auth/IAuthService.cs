using EmailMarketing.Data;

namespace EmailMarketing.Servicos.Auth
{
    public interface IAuthService
    {
        string GenerateJwtToken(ApplicationUser user);
    }
}
