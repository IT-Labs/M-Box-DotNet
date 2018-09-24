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
        private readonly IS3Manager _s3Manager;
        private readonly IEmailsManager _emailsManager;
        public ArtistsController(IRepository repository, ISongManager songManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager, IS3Manager s3Manager) : base(userManager)
        {
            _songManager = songManager;
            _repository = repository;
            _s3Manager = s3Manager;
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
            var imageS3Name = string.Empty;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (uploadedFiles.Count != 0)
                var formFile = uploadedFiles[0];
                if (formFile.Length > Math.Pow(1024, 2) * 3){
                	return View(model);
                }

                var path = Path.GetFullPath(formFile.FileName);
            if (ytLink.ToLower().StartsWith("www") || ytLink.ToLower().StartsWith("y"))
                ytLink = "https://" + ytLink;

            var vimeoLink = model.VimeoLink;
            if (vimeoLink.ToLower().StartsWith("www") || vimeoLink.ToLower().StartsWith("v"))
                vimeoLink = "https://" + vimeoLink;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    formFile.CopyToAsync(stream);
                }

                var uploadedImageName = _s3Manager.UploadFileAsync(path);
                imageS3Name = uploadedImageName.Result;

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                
            }
            if (string.IsNullOrEmpty(imageS3Name))
            {
                imageS3Name = "DefaultSong.jpg";
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
            var song = _repository.GetOne<Song>(filter: x => x.ArtistId == CurrentLoggedUserId && x.Id == songId);
            _s3Manager.DeleteFile(song.Picture);
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
        [HttpPost]
        public IActionResult ChangePicture(List<IFormFile> uploadedFiles)
        {
            var imageS3Name = string.Empty;
            if (uploadedFiles.Count == 0)
            {
                return RedirectToAction("MyAccount");
            }
            var formFile = uploadedFiles[0];

            if (formFile.Length > Math.Pow(1024, 2) * 3)
            {
                return RedirectToAction("MyAccount");
            }

            var path = Path.GetFullPath(formFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                formFile.CopyToAsync(stream);
            }

            var uploadedImageName = _s3Manager.UploadFileAsync(path);
            imageS3Name = uploadedImageName.Result;

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var currentUser = _userManager.FindByIdAsync(CurrentLoggedUserId.ToString()).Result;
            currentUser.Picture = imageS3Name;
            _userManager.UpdateAsync(currentUser);
            return RedirectToAction("MyAccount");
        }
    }
}