using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Data.Repositories
{
    public class EmailTemplatesRepository : IEmailTemplatesRepository
    {
        private MBoxDbContext _bmoxDbContext;

        public EmailTemplatesRepository(MBoxDbContext bmoxDbContext)
        {
            _bmoxDbContext = bmoxDbContext;
        }
        public EmailTemplate getEmailTemplateByType(EmailTemplates type)
        {
            return _bmoxDbContext.EmailTemplates.FirstOrDefault(x => x.Type == type);
        }
    }
}
