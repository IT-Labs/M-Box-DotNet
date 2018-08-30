using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class EmailTemplate : AuditableEntity
    {
        public virtual EmailTemplates Type { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
