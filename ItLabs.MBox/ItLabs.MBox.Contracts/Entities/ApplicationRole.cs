using ItLabs.MBox.Contracts.Enums;
using Microsoft.AspNetCore.Identity;

namespace ItLabs.MBox.Contracts.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public virtual Roles Type { get; set; }
    }
}
