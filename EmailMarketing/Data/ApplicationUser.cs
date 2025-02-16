using Microsoft.AspNetCore.Identity;


namespace EmailMarketing.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }

        public string Email { get; set; }
    }
}
