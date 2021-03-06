﻿using ItLabs.MBox.Contracts.Enums;

namespace ItLabs.MBox.Contracts.Entities
{
    public class EmailTemplate : Entity
    {
        public virtual EmailTemplateType Type { get; set; }

        public virtual string Name { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual string LinkText { get; set; }
    }
}
