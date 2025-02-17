namespace EmailMarketing.Servicos.Email
{
    public interface IEmailHelper
    {
        Task LogEmailAsync(string email, string status, string mensagemErro, string stackTrace);

    }
}
