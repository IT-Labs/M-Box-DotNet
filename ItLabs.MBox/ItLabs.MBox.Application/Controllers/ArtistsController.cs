﻿using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly ISongManager _songManager;
        private readonly IS3Manager _s3Manager;
        private readonly IEmailsManager _emailsManager;
        private readonly IArtistManager _artistManager;
        public ArtistsController(IArtistManager artistManager,ISongManager songManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager, IS3Manager s3Manager) : base(userManager)
        {
            _songManager = songManager;
            _s3Manager = s3Manager;
            _emailsManager = emailsManager;
            _artistManager = artistManager;
        }
        public IActionResult Index()
        {
            var model = new PagingModel<Song>();
            model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, string.Empty);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetArtistSongs([FromQuery] PagingModel<Song> model)
        {
            model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, model.SearchQuery);

            return View("NextSongs", model);
        }

        public IActionResult Search(string searchValue)
        {
            var model = new PagingModel<Song>();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                model.PagingList = _songManager.GetArtistSongs(CurrentLoggedUserId, model.Skip, model.Take, searchValue);
                return View("Index", model);
            }

            return RedirectToAction("Index");
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
            string imageS3Name = null;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (uploadedFiles.Count > 0)
            {
                var formFile = uploadedFiles[0];
                if (formFile.Length > MBoxConstants.MaximumImageSizeAllowed)
                {
                    ModelState.AddModelError("Picture", "Maximum 3MB picture size allowed!");
                    return View(model);
                }
                imageS3Name = _s3Manager.UploadFile(formFile);
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
                Picture = imageS3Name,
                ArtistId = CurrentLoggedUserId
            }, CurrentLoggedUserId);

            _songManager.Save();

            return View("SuccessfullyPublishedSong");

        }
        [HttpPost]
        public IActionResult DeleteSong(int songId)
        {
            var song = _songManager.GetOne(filter: x => x.ArtistId == CurrentLoggedUserId && x.Id == songId);
            _songManager.Delete(song);
            _songManager.Save();

            return RedirectToAction("Index");
        }

        public IActionResult EditSongDetails(int songId)
        {
            var songObject = _songManager.GetOne(filter: x => x.ArtistId == CurrentLoggedUserId && x.Id == songId);
            var song = new AddNewSongViewModel()
            {
                AlbumName = songObject.AlbumName,
                GenreName = songObject.Genre,
                ReleaseDate = songObject.ReleaseDate,
                SongLyrics = songObject.Lyrics,
                SongName = songObject.Name,
                VimeoLink = songObject.VimeoLink,
                YoutubeLink = songObject.YouTubeLink,
                Picture = songObject.PictureName,
            };

            return View(song);
        }
        [HttpPost]
        public IActionResult ChangePicture(List<IFormFile> uploadedFiles)
        {
            var model = new MyAccountViewModel();
            var imageS3Name = string.Empty;
            if (uploadedFiles.Count == 0)
            {
                ModelState.AddModelError("Picture", "Please choose a picture!");
                return View("MyAccount", model);
            }
            var formFile = uploadedFiles[0];

            if (formFile.Length > MBoxConstants.MaximumImageSizeAllowed)
            {
                //Error Message
                ModelState.AddModelError("Picture", "Maximum 3MB picture size allowed!");
                return View("MyAccount", model);
            }

            imageS3Name = _s3Manager.UploadFile(formFile);

            var currentUser = _userManager.FindByIdAsync(CurrentLoggedUserId.ToString()).Result;
            currentUser.Picture = imageS3Name;
            _userManager.UpdateAsync(currentUser).Wait();
            return RedirectToAction("MyAccount");
        }

        
    }
}