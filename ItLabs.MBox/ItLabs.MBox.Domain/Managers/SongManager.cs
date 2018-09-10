using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class SongManager : ISongManager
    {
        private IRepository _repository;

        public SongManager(IRepository repository)
        {
            _repository = repository;
        }
        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _repository.GetAll<Song>(includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.ReleaseDate), take: number).ToList();

        }

        public IList<Song> GetRecentlyAddedSongsOfMostPopularArtist(int number)
        {
            var mostFollowedArtist = _repository.GetAll<Artist>(orderBy: x => x.OrderByDescending(y => y.Follows.Count)).FirstOrDefault();
            var r = nameof(Artist.User);
            return _repository.Get<Song>(filter: x => x.Artist == mostFollowedArtist, includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.ReleaseDate), take: number)
                .ToList();


        }
    }
}
