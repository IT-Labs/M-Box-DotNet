using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Dtos
{
    public class MailDto
    {
        public string EmailAddress { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

    }
}
