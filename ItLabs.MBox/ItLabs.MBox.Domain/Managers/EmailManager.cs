using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;

namespace ItLabs.MBox.Domain.Managers
{
    public class EmailManager : IEmailsManager
    {
        private IRepository _repository;
        private readonly string SourceEmailAddress = "no-reply@it-labs.com";
        private readonly string username = "AKIAJHEYUTQZO5EDB3WA";
        private readonly string password = "Akp4SGKhVhC/SAjV+bao5XocI7A+yl7s6/Q7e/Wa3ffR";
        private readonly string host = "email-smtp.us-east-1.amazonaws.com";
        private readonly int port = 25;

        public EmailManager(IRepository repository)
        {
            _repository=repository;
        }
        public Task SendMail(EmailTemplateType type, string email, string callbackUrl)
        {
            //zemanje credentials
            var SourceEmailAddress = _repository.GetAll<Configuration>().Where(c => c.Key == ConfigurationKey.AwsSesFromAddress).FirstOrDefault().Value;
            var template = _repository.GetAll<EmailTemplate>().Where(c => c.Type == type).FirstOrDefault();
            var user = _repository.GetAll<ApplicationUser>().Where(x => x.Email == email).FirstOrDefault();
            var bodyToSend = template.Body.Replace("[Name]", user.Name);
            if (bodyToSend.Contains("[Link]"))
                bodyToSend = bodyToSend.Replace("[Link]", "<a href=" + callbackUrl + ">" + template.LinkText + "</a>");
            SendAMail(username,password,email,template.Subject,bodyToSend);

            return Task.CompletedTask;
        }

        public void SentContactFormMail(string name, string email, string message)
        {
            //zemanje credentials
            var template = _repository.GetAll<EmailTemplate>().Where(x => x.Type == EmailTemplateType.ContactForm).FirstOrDefault();
            var bodyToSend = message+ "<br> <br> Email sent by:<br>Name: " + name + "<br>Email Address: "+email;
            SendAMail(username, password, email, template.Subject, bodyToSend);
        }
        public void SendAMail(string username, string password,string email,string subject,string bodyToSend)
        {
            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;
                var msg = new MailMessage(
                    SourceEmailAddress,
                          email,
                          subject,
                          bodyToSend
                    )
                {
                    IsBodyHtml = true
                };
                client.Send(msg);
            }
        }
    }
}
