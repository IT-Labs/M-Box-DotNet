﻿using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Artist : IAuditCreate, IAuditUpdate
    {
        public virtual ApplicationUser Id { get; set; }

        public string Bio { get; set; }

        public Boolean IsDeleted { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
