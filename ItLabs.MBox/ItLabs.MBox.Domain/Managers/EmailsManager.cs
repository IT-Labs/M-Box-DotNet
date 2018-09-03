using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Domain.Managers
{
    public class EmailsManager : IEmailManager
    {
        private readonly IRepository<ApplicationUser> _usersRepository;
        private readonly IRepository<EmailTemplate> _emailTemplatesRepository;
        private readonly IRepository<Configuration> _configurationRepository;


        public EmailsManager(IRepository<ApplicationUser> usersRepository, 
            IRepository<EmailTemplate> emailTemplatesRepository,
            IRepository<Configuration> configurationRepository)
        {
            _usersRepository = usersRepository;
            _emailTemplatesRepository = emailTemplatesRepository;
            _configurationRepository = configurationRepository;
        }
        public Task SendMail(EmailTemplates type, string email, string callbackUrl)
        {
            var SourceEmailAddress = _configurationRepository.GetAll().Where(c => c.Key == nameof(SourceEmails.ItLabsEmail)).FirstOrDefault().Value;
            var template = _emailTemplatesRepository.GetAll().Where(c => c.Type == type).FirstOrDefault();
            var user = _usersRepository.GetAll().Where(x => x.Email == email).FirstOrDefault();
            var bodyToSend = template.Body.Replace("[Name]", user.Name);
            if (bodyToSend.Contains("[Link]"))
                bodyToSend = bodyToSend.Replace("[Link]", "<a href=" + callbackUrl + ">" + template.LinkText + "</a>");
            var username = "AKIAJHEYUTQZO5EDB3WA";  // Replace with your SMTP username.
            var password = "Akp4SGKhVhC/SAjV+bao5XocI7A+yl7s6/Q7e/Wa3ffR";  // Replace with your SMTP password.
            var host = "email-smtp.us-east-1.amazonaws.com";
            var port = 25;

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;
                var msg = new MailMessage(
                    SourceEmailAddress,  // Replace with the sender address.
                          email,    // Replace with the recipient address.
                          template.Subject,
                          bodyToSend
                    );
                msg.IsBodyHtml = true;
                client.Send(msg);
            }

            return Task.CompletedTask;
        }
    }
}
