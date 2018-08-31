using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ItLabs.MBox.Domain.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        IUsersRepository _usersRepository;
        IEmailTemplatesRepository _emailTemplatesRepository;

        private readonly string SourceEmailAddress = "no-reply@it-labs.com";

        public EmailSender(IUsersRepository usersRepository, IEmailTemplatesRepository emailTemplatesRepository)
        {
            _usersRepository = usersRepository;
            _emailTemplatesRepository = emailTemplatesRepository;

        }
        public Task SendMail(EmailTemplates type, string email, string callbackUrl)
        {
            var template = _emailTemplatesRepository.getEmailTemplateByType(type);
            var user = _usersRepository.GetUserByEmail(email);
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
                MailMessage msg = new MailMessage(
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
