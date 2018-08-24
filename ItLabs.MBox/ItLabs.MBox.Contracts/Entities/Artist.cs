using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Artist : IAuditable
    {
        public int ArtistId { get; set; }

        public string Bio { get; set; }

        public Boolean IsDeleted { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        //public RecordLabel RecordLabel { get; set; }
        //public int RecordLabelId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        //public int ApplicationUserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
