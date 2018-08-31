using ItLabs.MBox.Contracts.Entities;
using System.Linq;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistsRepository
    {
        IQueryable<Artist> GetAllArtists();
    }
}
