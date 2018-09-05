using ItLabs.MBox.Contracts.Data_Transfer_Objects;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistsManager
    {
        IList<Artist> GetAllArtists();
        IList<ArtistDto> GetMostFollowedArtists(int number);
    }
}