using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistsManager
    {
        private ArtistsRepository _ArtistsRepostiory;

        public ArtistsManager()
        {
            _ArtistsRepostiory = new ArtistsRepository();
        }
        public IList<Artist> GetAllArtists()
        {
            return null;
        }
    }
}
