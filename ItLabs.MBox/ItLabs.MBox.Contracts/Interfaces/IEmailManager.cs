using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IEmailsManager : IBaseManager<EmailTemplate>
    {
        void PrepareSendMail(EmailTemplateType type, string email, string callbackUrl);
        void PrepareContactFormMail(string name, string email, string message);
        void SendMultipleMails(IList<MailDto> mailingList, IList<Configuration> configuration);
        IList<Configuration> GetConfiguration();
    }
}
