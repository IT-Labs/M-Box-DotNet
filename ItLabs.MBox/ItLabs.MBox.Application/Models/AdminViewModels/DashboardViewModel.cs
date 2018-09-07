using ItLabs.MBox.Contracts.Data_Transfer_Objects;
using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.AdminViewModels
{
    public class DashboardViewModel
    {
        public IList<RecordLabelNumberOfArtistsDto> GetNumberOfArtists { get; set; }
    }
}
