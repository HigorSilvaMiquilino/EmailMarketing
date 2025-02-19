using System.Threading.Tasks;

namespace EmailMarketing.Servicos.Recuperacao
{
    public interface IRecuperacaoService
    {
        Task<bool> getEmail(string email);
    }
}
