using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly IRepository _repository;
        private readonly ISongManager _songManager;
        private readonly IEmailsManager _emailsManager;
        public ArtistsController(IRepository repository, ISongManager songManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager) : base(userManager)
        {
            _songManager = songManager;
            _repository = repository;
            _emailsManager = emailsManager;
        }
        public IActionResult Index()
        {
            var model = new PagingModel<Song>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, string.Empty);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetArtistSongs([FromQuery] PagingModel<Song> model)
        {
            if (string.IsNullOrEmpty(model.SearchQuery) || string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, string.Empty);
            }
            else
            {
                model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, model.SearchQuery);
            }

            return View("NextSongs", model);
        }

        public IActionResult Search(string searchValue)
        {
            var model = new PagingModel<Song>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, searchValue);
                return View("Index", model);
            }

            return RedirectToAction("Index", "Artists");
        }



        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewSong()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewSong(AddNewSongViewModel model, List<IFormFile> uploadedFiles)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ytLink = model.YoutubeLink;
            if (ytLink.ToLower().StartsWith("www") || ytLink.ToLower().StartsWith("y"))
                ytLink = "https://" + ytLink;

            var vimeoLink = model.VimeoLink;
            if (vimeoLink.ToLower().StartsWith("www") || vimeoLink.ToLower().StartsWith("v"))
                vimeoLink = "https://" + vimeoLink;

            _songManager.Create(new Song()
            {
                

                Name = model.SongName,
                AlbumName = model.AlbumName,
                Genre = model.Genres.ToString(),
                VimeoLink = vimeoLink,
                YouTubeLink = ytLink,
                ReleaseDate = model.ReleaseDate,
                Lyrics = model.SongLyrics,
                ArtistId = CurrentLoggedUserId
            }, CurrentLoggedUserId);

            _songManager.Save();

            return View("SuccessfullyPublishedSong");
        }

        [HttpPost]
        public IActionResult DeleteSong(int songId)
        {
            var song = _repository.GetOne<Song>(filter: x => x.ArtistId == CurrentLoggedUserId && x.Id == songId);

            _repository.Delete(song);
            _repository.Save();

            return RedirectToAction("Index");
        }

        public IActionResult EditSongDetails(int songId)
        {
            var song = new AddNewSongViewModel() { };
            song.Song = _repository.GetOne<Song>(filter: x => x.ArtistId == CurrentLoggedUserId && x.Id == songId);

            return View("EditSongDetails", song);
        }
    }
}