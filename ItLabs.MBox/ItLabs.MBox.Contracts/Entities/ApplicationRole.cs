using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ItLabs.MBox.Contracts.Entities
{
    public class ApplicationRole : IdentityRole<int>, IEntity
    {
        public virtual Roles Type { get; set; }
    }
}
