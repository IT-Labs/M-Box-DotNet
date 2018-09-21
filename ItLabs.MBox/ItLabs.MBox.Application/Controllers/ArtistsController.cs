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

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly IRepository _repository;
        private readonly ISongManager _songManager;
        private readonly IS3Manager _s3Manager;
        private readonly IEmailsManager _emailsManager;
        private readonly IConfigurationManager _configurationManager;
        public ArtistsController(IRepository repository, ISongManager songManager, UserManager<ApplicationUser> userManager, IS3Manager s3Manager, IEmailsManager emailsManager, IConfigurationManager configurationManager) : base(userManager)
        {
            _songManager = songManager;
            _repository = repository;
            _s3Manager = s3Manager;
            _emailsManager = emailsManager;
            _configurationManager = configurationManager;
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
            /*
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (uploadedFiles.Count != 0)
            {
                if(uploadedFiles[0].Length > Math.Pow(1024, 2) * 3)
                {
                    return View(model);
                }
                var fullpath = Path.GetTempFileName() + uploadedFiles[0].FileName;
                
                _s3Manager.UploadFileAsync(fullpath,);

            }
            */
            _songManager.Create(new Song()
            {
                Name = model.SongName,
                AlbumName = model.AlbumName,
                Genre = model.Genres.ToString(),
                VimeoLink = model.VimeoLink,
                YouTubeLink = model.YoutubeLink,
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
    }
}