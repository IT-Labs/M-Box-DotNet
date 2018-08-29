using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IAuditUpdate
    {
        int ModifiedBy { get; set; }

        DateTime DateModified { get; set; }
    }
}
