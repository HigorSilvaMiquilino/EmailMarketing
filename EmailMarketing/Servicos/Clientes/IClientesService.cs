using EmailMarketing.Data;

namespace EmailMarketing.Servicos.Clientes
{
    public interface IClientesService
    {
        Task<List<Cliente>> GetClientesPorArquivoNome(string arquivoNome);

        Task<List<string>> GetTodosArquivoNome();
    }
}
