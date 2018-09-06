using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelsManager
    {
        IList<RecordLabel> GetAllRecordLabels();
    }
}
