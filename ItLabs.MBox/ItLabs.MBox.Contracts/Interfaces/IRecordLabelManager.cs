using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRecordLabelManager : IBaseManager<RecordLabel>
    {
        void DeleteRecordLabel(ApplicationUser user);
        IList<RecordLabel> SearchRecordLabels(string searchValue, int toSkip, int toTake);
        AddMultipleArtistsDto ValidateCsvFile(IFormFile formFile, int recordLabelId);
        List<Artist> CreateMultipleArtists(IList<ApplicationUser> usersToBeAdded, int recordLabelId);
        int GetNumberOfArtists(int recordLabelId);
    }
}
