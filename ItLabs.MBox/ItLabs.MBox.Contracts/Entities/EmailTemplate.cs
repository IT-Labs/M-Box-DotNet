using ItLabs.MBox.Contracts.Enums;

namespace ItLabs.MBox.Contracts.Entities
{
    public class EmailTemplate : AuditableEntity
    {
        public virtual EmailTemplates Type { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string LinkText { get; set; }
    }
}
