using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class EmailTemplate : IAuditable
    {
        public int Id { get; set; }

        public EmailTemplates Type { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
