using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelManager : IBaseManager<RecordLabel>
    {
        IList<RecordLabel> GetAllRecordLabels();
        IList<RecordLabel> GetNextRecordLabels(int skip, int take);
        void DeleteRecordLabel(ApplicationUser user);
        IList<RecordLabel> GetSearchedRecordLabels(string searchValue, int toSkip, int toTake);
    }
}
