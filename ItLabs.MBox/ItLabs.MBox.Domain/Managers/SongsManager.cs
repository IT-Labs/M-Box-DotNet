using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class SongsManager : ISongsManager
    {
        private IRepository<Song> _songsRepository;

        public SongsManager(IRepository<Song> repository)
        {
            _songsRepository = repository;
        }

        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _songsRepository.GetAll()
                .Include(c => c.Artist.User)
                .OrderByDescending(x => x.ReleaseDate)
                .Take(number)
                .ToList();
        }
    }
}
