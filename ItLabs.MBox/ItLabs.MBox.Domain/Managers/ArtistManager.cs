using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using ItLabs.MBox.Data;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistManager : BaseManager<Artist>, IArtistManager
    {
        private IRepository _repository;

        public ArtistManager(IRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public IList<Artist> GetRecordLabelArtists(int recordLabelId, int toSkip, int toTake)
        {

            return _repository.Get<RecordLabelArtist>(filter: x => x.RecordLabelId == recordLabelId,
                                            includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}",
                                            skip: toSkip,
                                            take: toTake).Select(x => x.Artist).ToList();
        }

        public IList<Artist> GetAllUserArtists()
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}").ToList();
        }

        public IList<Artist> GetNextArtists(int toSkip, int toTake)
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}", skip: toSkip, take: toTake).ToList();
        }

        public IList<Artist> GetAllArtists()
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.RecordLabelArtists)},{nameof(Artist)},{nameof(Artist.User)}").ToList();
        }
        public IList<Artist> GetMostFollowedArtists(int number)
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}," +
                                   $"{nameof(Artist.Follows)}," +
                                   $"{nameof(Artist.RecordLabelArtists)}.{nameof(RecordLabel)}.{nameof(RecordLabel.User)}",
                                   orderBy: x => x.OrderByDescending(y => y.Follows.Count),
                                   take: number).ToList();
        }
        public void AddArtistToRecordLabel(Artist artist, RecordLabel recordLabel)
        {
            _repository.Create(new RecordLabelArtist() { RecordLabel = recordLabel, Artist = artist }, 1);
            _repository.Save();
        }
    }
}
