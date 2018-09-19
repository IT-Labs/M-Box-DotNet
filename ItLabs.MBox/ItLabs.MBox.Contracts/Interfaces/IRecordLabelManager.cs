using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelManager : IBaseManager<RecordLabel>
    {
        //IList<RecordLabel> GetAllRecordLabels();
        //IList<RecordLabel> GetNextRecordLabels(int skip, int take);
        IList<Artist> DeleteRecordLabel(ApplicationUser user);
        IList<RecordLabel> GetSearchedRecordLabels(string searchValue, int toSkip, int toTake);
        AddMultipleArtistsDto ValidateCsvFile(IFormFile formFile, int recordLabelId);
        int CreateMultipleArtists(IList<Artist> artistsToBeAdded, int recordLabelId);
    }
}
