using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.RecordLabelViewModels
{
    public class DashboardViewModel : PagingModel<Artist>
    {
        public int RecordLabelId { get; set; }
    }
}
