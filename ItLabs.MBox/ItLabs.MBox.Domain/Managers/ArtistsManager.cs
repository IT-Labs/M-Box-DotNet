using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Data;
using ItLabs.MBox.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
