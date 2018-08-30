using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistsManager
    {
        IList<Artist> GetAllArtists();
    }
}