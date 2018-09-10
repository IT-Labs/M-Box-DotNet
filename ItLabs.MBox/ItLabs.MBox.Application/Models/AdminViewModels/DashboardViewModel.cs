using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Application.Models.AdminViewModels
{
    public class DashboardViewModel
    {
        public IList<RecordLabel> RecordLabels { get; set; }
    }
}
