using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class AboutViewModel
    {
        public IList<RecordLabel> WeCooperateWith { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
