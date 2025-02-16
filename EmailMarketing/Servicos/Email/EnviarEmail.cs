using EmailMarketing.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net.Sockets;

namespace EmailMarketing.Servicos.Email
{
    public class EnviarEmail : IEmailSender, IEmailHelper
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public EnviarEmail(IConfiguration configuration, ApplicationDbContext context)
        {
            _config = configuration;
            _context = context;
        }

        public async Task EnviarEmailslAsync(string email, string nome)
        {
            var fromAddress = _config["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _config["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_config["EmailSettings:Port"]);
            var appPassword = _config["EmailSettings:Password"];

            var templatePath = _config["EmailSettings:TemplatePath"];
            var emailTemplate = await File.ReadAllTextAsync(templatePath);

            var custumizedTamplate = emailTemplate.Replace("{UsauarioNome}", nome);

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Bem-vindo a nossa plataforma",
                Body = custumizedTamplate,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            using var cliente = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new System.Net.NetworkCredential(fromAddress, appPassword),
                EnableSsl = true
            };

            try
            {
                await cliente.SendMailAsync(message);
            }
            catch (SocketException ex)
            {
                await LogEmailAsync(email,
                    EmailStatusEnum.Erro.ToString(),
                    "Failed to connect to the SMTP server. Please verify your SMTP settings.");
                throw new SmtpException("Failed to connect to the SMTP server. Please verify your SMTP settings.", ex);
            }
            catch (SmtpException ex)
            {
                await LogEmailAsync(email,
                     EmailStatusEnum.Erro.ToString(),
                     "Failure sending mail.");
                throw new SmtpException("Failure sending mail.", ex);
            }
            catch (Exception ex)
            {
                await LogEmailAsync(email,
                        EmailStatusEnum.Erro.ToString(),
                        "An error occurred while sending the email.");
                throw new Exception("An error occurred while sending the email.", ex);
            }
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

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
