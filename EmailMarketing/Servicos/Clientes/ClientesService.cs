using EmailMarketing.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Servicos.Clientes
{
    public class ClientesService : IClientesService
    {
        private readonly ApplicationDbContext _context;
        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetClientesPorArquivoNome(string arquivoNome)
        {
            return await _context.Clientes
                .Where(c => c.ArquivoNome == arquivoNome)
                .ToListAsync();
        }

        public async Task<List<string>> GetTodosArquivoNome()
        {
            return await _context.Clientes
                .Where(c => c.ArquivoNome != null && c.ArquivoNome.Trim() != string.Empty)
                .Select(c => c.ArquivoNome)
                .Distinct()
                .ToListAsync();
        }
    }
}
