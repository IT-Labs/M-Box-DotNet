using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IEmailsManager
    {
        Task SendMail(EmailTemplates type, string email, string callbackUrl);
        void SentContactFormMail(string name, string email, string message);
    }
}
