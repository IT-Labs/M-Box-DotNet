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
    public class EmailManager : IEmailsManager
    {
        private IRepository _repository;
        private readonly string SourceEmailAddress;
        private readonly string username;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private readonly string testReceiverEmail;

        public EmailManager(IRepository repository)
        {
            _repository=repository;
            try
            {
                SourceEmailAddress = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.AwsSesFromAddress).FirstOrDefault().Value;
                username = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.AwsSesUsername).FirstOrDefault().Value;
                password = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.AwsSesPassword).FirstOrDefault().Value;
                host = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.AwsSesHost).FirstOrDefault().Value;
                port = Int32.Parse(_repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.AwsSesPort).FirstOrDefault().Value);
                testReceiverEmail = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.TestReceiverEmail).FirstOrDefault().Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public Task SendMail(EmailTemplateType type, string email, string callbackUrl)
        {
            //zemanje credentials
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
            var recieverMail = _repository.Get<Configuration>(filter: x => x.Key == ConfigurationKey.ContactFormRecieverMail).FirstOrDefault().Value;
            var template = _repository.GetAll<EmailTemplate>().Where(x => x.Type == EmailTemplateType.ContactForm).FirstOrDefault();
            var bodyToSend = message+ "<br> <br> Sender:<br>Name: " + name + "<br>Email Address: "+email;
            SendAMail(username, password, recieverMail, template.Subject, bodyToSend);
        }
        public void SendAMail(string username, string password,string email,string subject,string bodyToSend)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var isDevelopment = environment == EnvironmentName.Development;

            if (environment == EnvironmentName.Development || environment == EnvironmentName.Staging)
            {
                //bodyToSend.Prepend("This is test mail. Original emal ade:" + email);
                email = testReceiverEmail;               
            }

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
