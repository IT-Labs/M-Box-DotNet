using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IAuditCreate
    {
        int CreatedBy { get; set; }

        DateTime DateCreated { get; set; }
    }
}
