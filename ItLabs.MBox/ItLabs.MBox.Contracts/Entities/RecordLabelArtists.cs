using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabelArtists : IAuditable
    {
        public int RecordLabelArtistsId { get; set; }

        public virtual Artist Artist { get; set; }
        //public int ArtistId { get; set; }
        public virtual RecordLabel RecordLabel { get; set; }
        //public int RecordLabelId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
