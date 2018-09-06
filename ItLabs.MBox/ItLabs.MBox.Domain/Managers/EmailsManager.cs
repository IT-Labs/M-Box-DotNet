using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Domain.Managers
{
    public class EmailsManager : IEmailsManager
    {
        private IRepository<ApplicationUser> _usersRepository;
        private IRepository<EmailTemplate> _emailTemplatesRepository;
        private IRepository<Configuration> _configurationRepository;
        private readonly string SourceEmailAddress = "no-reply@it-labs.com";
        private readonly string username = "AKIAJHEYUTQZO5EDB3WA";
        private readonly string password = "Akp4SGKhVhC/SAjV+bao5XocI7A+yl7s6/Q7e/Wa3ffR";
        private readonly string host = "email-smtp.us-east-1.amazonaws.com";
        private readonly int port = 25;

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
            

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;
                var msg = new MailMessage(
                    SourceEmailAddress,  
                          email,    
                          template.Subject,
                          bodyToSend
                    );
                msg.IsBodyHtml = true;
                client.Send(msg);
            }

            return Task.CompletedTask;
        }

        public void SentContactFormMail(string name, string email, string message)
        {
            var template = _emailTemplatesRepository.GetAll().Where(x => x.Type == EmailTemplates.ContactForm).FirstOrDefault();
            var bodyToSend = message+ "<br> <br> Email sent by:<br>Name: " + name + "<br>Email Address: "+email;
            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;
                var msg = new MailMessage(
                    SourceEmailAddress,  
                          email,    
                          template.Subject,
                          bodyToSend
                    );
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
        }
    }
}
