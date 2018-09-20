using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using ItLabs.MBox.Data;
using System;
using System.Linq.Expressions;
using ItLabs.MBox.Contracts.Enums;
using System.Threading.Tasks;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistManager : BaseManager<Artist>, IArtistManager
    {
        private IRepository _repository;
        private IEmailsManager _emailsManager;

        public ArtistManager(IRepository repository, IEmailsManager emailsManager) : base(repository)
        {
            _repository = repository;
            _emailsManager = emailsManager;
        }

        public IList<Artist> GetSearchedArtists(int recordLabelId, int toSkip, int toTake, string searchValue)
        {
            return _repository.Get<RecordLabelArtist>(
                filter: x => x.RecordLabelId == recordLabelId && x.Artist.IsDeleted == false && (x.Artist.User.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Artist.User.Email.ToUpper().Contains(searchValue.ToUpper())),
                includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}",
                skip: toSkip,
                take: toTake)
                .Select(x => x.Artist).ToList();
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
            _repository.Create(new RecordLabelArtist() { RecordLabel = recordLabel, Artist = artist }, recordLabel.Id);
            _repository.Save();
        }

        public void DeleteArtist(int recordLabelId, int artistlId)
        {
            var artist = _repository.GetOne<Artist>(x => x.Id == artistlId, includeProperties: $"{ nameof(Artist.User)}");
            var recordLabelArtist = _repository.GetOne<RecordLabelArtist>(x => x.Artist.Id == artistlId);

            artist.IsDeleted = true;
            _repository.Update<Artist>(artist, recordLabelId);

            _repository.Delete(recordLabelArtist);
            _repository.Save();

            _emailsManager.PerpareSendMail(EmailTemplateType.DeletedArtist, artist.User.Email, "");
        }
    }
}
