using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class ApplicationRole : IdentityRole<int>, IEntity
    {
        public virtual Role Type { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}
