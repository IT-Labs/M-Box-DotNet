using ItLabs.MBox.Contracts.Data_Transfer_Objects;
using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelsManager
    {
        IList<RecordLabel> GetAllRecordLabels();

        IList<RecordLabelNumberOfArtistsDto> RecordLabelNumberOfArtists();
    }
}
