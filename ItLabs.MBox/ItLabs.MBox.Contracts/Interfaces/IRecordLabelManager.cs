using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelManager : IBaseManager<RecordLabel>
    {
        void DeleteRecordLabel(ApplicationUser user);
        IList<RecordLabel> GetSearchedRecordLabels(string searchValue, int toSkip, int toTake);
        AddMultipleArtistsDto ValidateCsvFile(IFormFile formFile, int recordLabelId);
        int CreateMultipleArtists(IList<Artist> artistsToBeAdded, int recordLabelId);
        void prepareAndSendMails(IList<Artist> artists, ApplicationUser recordLabel);
    }
}
