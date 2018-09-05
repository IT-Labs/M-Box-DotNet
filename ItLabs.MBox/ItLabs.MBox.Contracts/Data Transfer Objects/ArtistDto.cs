using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Data_Transfer_Objects
{
    public class ArtistDto
    {
        public Artist Artist { get; set; }
        public RecordLabel RecordLabel { get; set; }
    }
}
