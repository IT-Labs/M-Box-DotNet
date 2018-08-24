using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }

        int CreatedBy { get; set; }
        int ModifiedBy { get; set; }
    }
}
