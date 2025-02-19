using EmailMarketing.Data;
using EmailMarketing.Models;

namespace EmailMarketing.Servicos.Home
{
    public interface IHomeService
    {
        Task<string> CreateAsync(RegistrarUserModel registerUserModel);

        Task<List<UsuarioViewModel>> GetAllAsync();

        // Task<List<EmailLog>> GetEmailLogsAsync();

        //Task dispararTodosEmailsAsync(List<string> selectedEmails);

        Task DeleteUserAsync(string userId);
        Task LogEmailAsync(string email, string status, string mensagemErro);

        Task<ApplicationUser> GetUsuarioEmailAsync(string email);

        Task<bool> ResetSenhaAsync(ApplicationUser user, string model);
    }
}
