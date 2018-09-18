using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Hosting;

namespace ItLabs.MBox.Domain.Managers
{
    public class EmailManager : BaseManager<EmailTemplate>, IEmailsManager
    {
        private readonly IRepository _repository;


        public EmailManager(IRepository repository) : base(repository)
        {
            _repository = repository;

        }
        public Task PerpareSendMail(EmailTemplateType type, string email, string callbackUrl)
        {
            var template = _repository.GetAll<EmailTemplate>().Where(c => c.Type == type).FirstOrDefault();
            var user = _repository.GetAll<ApplicationUser>().Where(x => x.Email == email).FirstOrDefault();
            var bodyToSend = template.Body.Replace("[Name]", user.Name);
            if (bodyToSend.Contains("[Link]"))
                bodyToSend = bodyToSend.Replace("[Link]", "<a href=" + callbackUrl + ">" + template.LinkText + "</a>");
            SendMailSmtp(email, template.Subject, bodyToSend);
            return Task.CompletedTask;
        }

        public void PrepareContactFormMail(string name, string email, string message)
        {
            var recieverMail = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.ContactFormRecieverMail).FirstOrDefault().Value;
            var template = _repository.GetAll<EmailTemplate>().Where(x => x.Type == EmailTemplateType.ContactForm).FirstOrDefault();
            var bodyToSend = message + "<br> <br> Sender:<br>Name: " + name + "<br>Email Address: " + email;
            SendMailSmtp(recieverMail, template.Subject, bodyToSend);
        }


        public void SendMailSmtp(string email, string subject, string bodyToSend)
        {
            var configuration = _repository.GetAll<Configuration>();
            var awsSesFromAddress = configuration.Where(x => x.Key == ConfigurationKey.AwsSesFromAddress).FirstOrDefault().Value;
            var awsSesUsername = configuration.Where(x => x.Key == ConfigurationKey.AwsSesUsername).FirstOrDefault().Value;
            var awsSesPassword = configuration.Where(x => x.Key == ConfigurationKey.AwsSesPassword).FirstOrDefault().Value;
            var awsSesHost = configuration.Where(x => x.Key == ConfigurationKey.AwsSesHost).FirstOrDefault().Value;
            var awsSesPort = Int32.Parse(configuration.Where(x => x.Key == ConfigurationKey.AwsSesPort).FirstOrDefault().Value);
            var testReceiverEmail = configuration.Where(x => x.Key == ConfigurationKey.TestReceiverEmail).FirstOrDefault().Value;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == EnvironmentName.Development || environment == EnvironmentName.Staging)
                email = testReceiverEmail;

            using (var client = new SmtpClient(awsSesHost, awsSesPort))
            {
                client.Credentials = new NetworkCredential(awsSesUsername, awsSesPassword);
                client.EnableSsl = true;
                var msg = new MailMessage(
                          awsSesFromAddress,
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
