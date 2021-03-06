﻿using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistManager : BaseManager<Artist>, IArtistManager
    {
        private readonly IRepository _repository;
        private readonly IEmailsManager _emailsManager;
        private readonly IS3Manager _s3Manager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistManager(IRepository repository, IEmailsManager emailsManager, IS3Manager s3Manager, ILogger<Artist> logger, UserManager<ApplicationUser> userManager) : base(repository,logger)
        {
            _repository = repository;
            _emailsManager = emailsManager;
            _s3Manager = s3Manager;
            _userManager = userManager;
        }

        public IList<Artist> GetRecordLabelArtists(int? recordLabelId, int toSkip, int toTake, string searchValue)
        {
            if(searchValue == null)
            {
                searchValue = "";
            }
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

        public void DeleteArtist(int recordLabelId, int artistlId)
        {
            var artist = _repository.GetOne<Artist>(x => x.Id == artistlId, includeProperties: $"{ nameof(Artist.User)}");
            var recordLabelArtist = _repository.GetOne<RecordLabelArtist>(x=>x.Artist.Id == artistlId);
            artist.IsDeleted = true;
            try
            {
                _repository.Update<Artist>(artist, recordLabelId);
                _repository.Delete(recordLabelArtist);
                _repository.Save();
                _emailsManager.PrepareSendMail(EmailTemplateType.DeletedArtist, artist.User.Email, "");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            
        }
        public void ToggleFollow(int artistId, int followerId)
        {
            var artist = _repository.GetOne<Artist>(filter: x => x.Id == artistId, includeProperties: $"{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            var user = _userManager.FindByIdAsync(followerId.ToString()).Result;
            if (artist.Follows.Select(x => x.Follower).Contains(user))
            {
                artist.Follows.Remove(artist.Follows.Where(x => x.Follower == user).FirstOrDefault());
            }
            else
            {
                artist.Follows.Add(new Follow() { Artist = artist, Follower = user });
            }
            _repository.Update(artist, followerId);
            _repository.Save();
        }
    }
}
