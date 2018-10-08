using ItLabs.MBox.Contracts.Data_Structures;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class SearchManager : ISearchManager
    {
        public IRepository _repository { get; set; }
        public SearchManager(IRepository repository)
        {
            _repository = repository;
        }
        public PriorityQueue<object> Search(string searchValue, SearchType type)
        {
            var results = new PriorityQueue<object>();

            var exactSongs = new List<Song>();
            var startsWithSongs = new List<Song>();
            var endswithOrContainsSongs = new List<Song>();
            var exactArtists = new List<Artist>();
            var startsWithArtists = new List<Artist>();
            var endswithOrContainsArtists = new List<Artist>();
            var exactRecordLabels = new List<RecordLabel>();
            var startsWithRecordLabels = new List<RecordLabel>();
            var endswithOrContainsRecordLabels = new List<RecordLabel>();
            var containsLyricsSongs = new List<Song>();

            if (type == SearchType.SongName || type == SearchType.MostRelevant)
            {
                exactSongs = _repository.Get<Song>(filter: x => x.Name.ToLower().Equals(searchValue), includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList();
                startsWithSongs = _repository.Get<Song>(filter: x => x.Name.ToLower().StartsWith(searchValue), includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList();
                endswithOrContainsSongs = _repository.Get<Song>(filter: x => x.Name.ToLower().EndsWith(searchValue) || x.Name.ToLower().Contains(searchValue), includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList();
            }
            if (type == SearchType.ArtistName || type == SearchType.MostRelevant)
            {
                exactArtists = _repository.Get<Artist>(filter: x => x.User.Name.ToLower().Equals(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
                startsWithArtists = _repository.Get<Artist>(filter: x => x.User.Name.ToLower().StartsWith(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
                endswithOrContainsArtists = _repository.Get<Artist>(filter: x => x.User.Name.ToLower().EndsWith(searchValue) || x.User.Name.ToLower().Contains(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
            }
            if (type == SearchType.RecordLabelName || type == SearchType.MostRelevant)
            {
                exactRecordLabels = _repository.Get<RecordLabel>(filter: x => x.User.Name.ToLower().Equals(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
                startsWithRecordLabels = _repository.Get<RecordLabel>(filter: x => x.User.Name.ToLower().StartsWith(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
                endswithOrContainsRecordLabels = _repository.Get<RecordLabel>(filter: x => x.User.Name.ToLower().EndsWith(searchValue) || x.User.Name.ToLower().Contains(searchValue), includeProperties: $"{nameof(Artist.User)}").ToList();
            }
            if (type == SearchType.Lyrics || type == SearchType.MostRelevant)
            {
                containsLyricsSongs = _repository.Get<Song>(filter: x => x.Lyrics.ToLower().Contains(searchValue), includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList();
            }

            #region Enqueue Items

            foreach (var song in exactSongs)
            {
                results.Enqueue(song, 10);
            }
            foreach (var artist in exactArtists)
            {
                results.Enqueue(artist, 9);
            }
            foreach (var rl in exactRecordLabels)
            {
                results.Enqueue(rl, 8);
            }

            foreach (var song in startsWithSongs)
            {
                results.Enqueue(song, 7);
            }
            foreach (var artist in startsWithArtists)
            {
                results.Enqueue(artist, 6);
            }
            foreach (var rl in startsWithRecordLabels)
            {
                results.Enqueue(rl, 5);
            }

            foreach (var song in endswithOrContainsSongs)
            {
                results.Enqueue(song, 4);
            }
            foreach (var artist in endswithOrContainsArtists)
            {
                results.Enqueue(artist, 3);
            }
            foreach (var rl in endswithOrContainsRecordLabels)
            {
                results.Enqueue(rl, 2);
            }

            foreach (var song in containsLyricsSongs)
            {
                results.Enqueue(song, 1);
            }
            #endregion

            return results;
        }
    }
}
