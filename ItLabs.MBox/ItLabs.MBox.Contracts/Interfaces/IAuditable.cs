using System;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }

        int CreatedBy { get; set; }
        int ModifiedBy { get; set; }
    }
}
