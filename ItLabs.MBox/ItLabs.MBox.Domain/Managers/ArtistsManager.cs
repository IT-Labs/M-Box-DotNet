using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistsManager : IArtistsManager
    {
        private IArtistsRepository _artistsRepostiory;

        public ArtistsManager(IArtistsRepository repository)
        {
            _artistsRepostiory = repository;
        }
        public IList<Artist> GetAllArtists()
        {
            return _artistsRepostiory.GetAllArtists().ToList();
        }
    }
}
