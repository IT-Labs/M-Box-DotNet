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
    public class SongManager : BaseManager<Song>, ISongManager
    {
        private IRepository _repository;

        public SongManager(IRepository repository):base(repository)
        {
            _repository = repository;
        }
        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _repository.GetAll<Song>(includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.ReleaseDate), take: number).ToList();

        }

        public IList<Song> GetMostPopularArtistSongs(int number)
        {
            var mostFollowedArtist = _repository.GetAll<Artist>(orderBy: x => x.OrderByDescending(y => y.Follows.Count)).FirstOrDefault();
            if (mostFollowedArtist == null) { return new List<Song>(); }
            return _repository.Get<Song>(filter: x => x.Artist == mostFollowedArtist, includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.ReleaseDate), take: number)
                .ToList();


        }

        public IList<Song> GetArtistSongs(int ArtistId, int toSkip, int toTake, string searchValue)
        {
            return _repository.Get<Song>(
                filter: x => x.ArtistId == ArtistId && (x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.AlbumName.ToUpper().Contains(searchValue.ToUpper()) || x.Genre.ToUpper().Contains(searchValue.ToUpper())),
                includeProperties: $"{nameof(Song.Artist)}",
                skip: toSkip,
                take: toTake
                ).ToList(); ;
        }
    }
}
