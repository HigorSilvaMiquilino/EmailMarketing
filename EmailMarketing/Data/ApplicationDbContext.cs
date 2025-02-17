using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<EmailLog> EmailLogs { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Promocao> Promocoes { get; set; }

        public DbSet<LogEnvio> LogsEnvio { get; set; }
        public DbSet<LogAbertura> LogsAbertura { get; set; }
        public DbSet<LogClique> LogsClique { get; set; }
        public DbSet<LogErro> LogsErro { get; set; }

    }
}