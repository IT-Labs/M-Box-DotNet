using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IEmailTemplatesRepository
    {
        EmailTemplate getEmailTemplateByType(EmailTemplates type);
    }
}
