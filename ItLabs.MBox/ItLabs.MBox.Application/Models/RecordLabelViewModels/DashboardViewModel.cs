using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.RecordLabelViewModels
{
    public class DashboardViewModel
    {
        public IList<Artist> Artists { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int RecordLabelId { get; set; }
    }
}
