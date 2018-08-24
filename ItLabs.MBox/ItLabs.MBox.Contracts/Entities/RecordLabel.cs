using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabel : IAuditable
    {
        public int RecordLabelId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string AboutInfo { get; set; }
        //public int ApplicationUserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
