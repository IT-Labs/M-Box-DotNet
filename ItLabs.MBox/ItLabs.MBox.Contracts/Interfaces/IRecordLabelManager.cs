using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelManager
    {
        IList<RecordLabel> GetAllRecordLabels();
        IList<RecordLabel> GetNextRecordLabels(int skip, int take);
    }
}
