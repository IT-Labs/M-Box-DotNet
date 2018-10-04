using ItLabs.MBox.Application.Models;
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
using System.Linq;
using System.Text.RegularExpressions;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly ISongManager _songManager;
        private readonly IS3Manager _s3Manager;
        private readonly IEmailsManager _emailsManager;
        private readonly IArtistManager _artistManager;
        public ArtistsController(IArtistManager artistManager, ISongManager songManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager, IS3Manager s3Manager) : base(userManager)
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
                if(!(formFile.ContentType.Equals("image/png") || formFile.ContentType.Equals("image/jpeg") || formFile.ContentType.Equals("image/jpeg")))
                {
                    ModelState.AddModelError("Picture", formFile.ContentType + " extension is not allowed. You can only upload jpg, jpeg or png.");
                    return View(model);
                }
                if (formFile.Length > MBoxConstants.MaximumImageSizeAllowed)
                {
                    ModelState.AddModelError("Picture", "Upload file exceeded the maximum file size limit of 3MB.");
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
            var song = FillSongDetails(songId);
                SongId = songObject.Id,

            return View(song);
        }
        [HttpPost]
        public IActionResult ChangePicture(List<IFormFile> uploadedFiles)
        {
            var model = new MyAccountViewModel();
            string imageS3Name = null;
            if (uploadedFiles.Count == 0)
            {
                ModelState.AddModelError("Picture", "Please choose a picture!");
                return View("MyAccount", model);
            }
            var formFile = uploadedFiles[0];
            if (!(formFile.ContentType.Equals("image/png") || formFile.ContentType.Equals("image/jpeg") || formFile.ContentType.Equals("image/jpeg")))
            {
                ModelState.AddModelError("Picture", formFile.ContentType + " extension is not allowed. You can only upload jpg, jpeg or png.");
                return View("MyAccount",model);
            }
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


        [HttpPost]
        public IActionResult EditName(string artistName, int artistlId)
        {
            var model = new MyAccountViewModel();
            var artist = _artistManager.GetOne(x => x.Id == artistlId, includeProperties: $"{ nameof(Artist.User)}");
            artistName = artistName.Trim();

            if (string.IsNullOrWhiteSpace(artistName) || artistName.Length < 2)
            {
                ModelState.AddModelError("Name", "The Name must contain at least 2 characters");
                return View("MyAccount", model);
            }
            if (artistName.Length > 50)
            {
                ModelState.AddModelError("Name", "The Name cannot contain more than 50 characters");
                return View("MyAccount", model);
            }

            artist.User.Name = artistName;
            _artistManager.Update(artist, artistlId);
            _artistManager.Save();

            return RedirectToAction("MyAccount");
        }

        [HttpPost]
        public IActionResult EditBio(string artistBio, int artistId)
        {
            var model = new MyAccountViewModel();
            var artist = _artistManager.GetOne(x => x.Id == artistId, includeProperties: $"{ nameof(Artist.User)}");

            if (string.IsNullOrWhiteSpace(artistBio))
                artistBio = "";

            artist.Bio = artistBio;
            _artistManager.Update(artist, artistId);
            _artistManager.Save();

            return RedirectToAction("MyAccount");
        }

        [HttpPost]
        public IActionResult EditSongName(string songName, int songId)
        {
            var song = _songManager.GetOne(x => x.Id == songId && x.ArtistId == CurrentLoggedUserId);
            songName = songName.Trim();
            song.Name = songName;
            var model = FillSongDetails(songId);

            if (string.IsNullOrWhiteSpace(songName) || songName.Length < 2)
            {
                ModelState.AddModelError("SongName", "The Song Name must contain at least 2 characters");
                return View("EditSongDetails", model);
            }
            if (songName.Length > 100)
            {
                ModelState.AddModelError("SongName", "The Song Name cannot contain more than 100 characters");
                return View("EditSongDetails", model);
            }
       
            _songManager.Update(song, CurrentLoggedUserId);
            _songManager.Save();

            return RedirectToAction("EditSongDetails", model);
        }

        [HttpPost]
        public IActionResult EditSongAlbum(string songAlbum, int songId)
        {
            var song = _songManager.GetOne(x => x.Id == songId && x.ArtistId == CurrentLoggedUserId);
            songAlbum = songAlbum.Trim();
            song.AlbumName = songAlbum;
            var model = FillSongDetails(songId);

            if (string.IsNullOrWhiteSpace(songAlbum) || songAlbum.Length < 2)
            {
                ModelState.AddModelError("AlbumName", "The Song Name must contain at least 2 characters");
                return View("EditSongDetails", model);
            }
            if (songAlbum.Length > 100)
            {
                ModelState.AddModelError("AlbumName", "The Song Name cannot contain more than 100 characters");
                return View("EditSongDetails", model);
            }

            _songManager.Update(song, CurrentLoggedUserId);
            _songManager.Save();

            return RedirectToAction("EditSongDetails", model);
        }

        [HttpPost]
        public IActionResult EditYoutubeLink(string youtubeLink, int songId)
        {
            var song = _songManager.GetOne(x => x.Id == songId && x.ArtistId == CurrentLoggedUserId);
            string urlRegex = @"^(http(s?)\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$";

            if (youtubeLink.ToLower().StartsWith("www") || youtubeLink.ToLower().StartsWith("y"))
                youtubeLink = "https://" + youtubeLink;

            song.YouTubeLink = youtubeLink;
            var model = FillSongDetails(songId);

            if (Regex.IsMatch(youtubeLink, urlRegex))
            {
                _songManager.Update(song, CurrentLoggedUserId);
                _songManager.Save();

                return RedirectToAction("EditSongDetails", model);
            }
            else
            {
                ModelState.AddModelError("YoutubeLink", "Please enter a valid Youtube link!");
                return View("EditSongDetails", model);
            }         
        }

        [HttpPost]
        public IActionResult EditVimeoLink(string vimeoLink, int songId)
        {
            var song = _songManager.GetOne(x => x.Id == songId && x.ArtistId == CurrentLoggedUserId);
            string urlRegex = @"^(http(s?)\:\/\/)?(www\.)?vimeo.com\/(?:channels\/(?:\w+\/)?|groups\/([^\/]*)\/videos\/|)(\d+)(?:|\/\?)";

            if (vimeoLink.ToLower().StartsWith("www") || vimeoLink.ToLower().StartsWith("v"))
                vimeoLink = "https://" + vimeoLink;

            song.VimeoLink = vimeoLink;
            var model = FillSongDetails(songId);

            if (Regex.IsMatch(vimeoLink, urlRegex))
            {
                _songManager.Update(song, CurrentLoggedUserId);
                _songManager.Save();

                return RedirectToAction("EditSongDetails", model);
            }
            else
            {
                ModelState.AddModelError("YoutubeLink", "Please enter a valid Vimeo link!");
                return View("EditSongDetails", model);
            }
        }

        [HttpPost]
        public IActionResult EditSongLyrics(string lyrics, int songId)
        {
            var song = _songManager.GetOne(x => x.Id == songId && x.ArtistId == CurrentLoggedUserId);

            if (string.IsNullOrWhiteSpace(lyrics))
                lyrics = "";

            lyrics = lyrics.Trim();
            song.Lyrics = lyrics;
            var model = FillSongDetails(songId);

            if(lyrics.Length > 10000)
            {
                ModelState.AddModelError("SongLyrics", "The Song Lyrics cannot contain more than 10000 characters");
                return View("EditSongDetails", model);
            }


        [HttpGet]
        public IActionResult Followers()
        {
            var model = new PagingModel<ApplicationUser>();
            var artist = _artistManager.GetOne(filter: x => x.Id == CurrentLoggedUserId, includeProperties: $"{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            model.PagingList = artist.Follows.Select(x => x.Follower).Skip(model.Skip).Take(model.Take).ToList();
            return View(model);
        }

            var model = new AddNewSongViewModel()
            {
                SongId = song.Id,
                AlbumName = song.AlbumName,
                GenreName = song.Genre,
                ReleaseDate = song.ReleaseDate,
                SongLyrics = song.Lyrics,
                SongName = song.Name,
                VimeoLink = song.VimeoLink,
                YoutubeLink = song.YouTubeLink,
                Picture = song.PictureName,
            };

            return model;
        }

        public IActionResult SearchFollowers(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return RedirectToAction("Followers");
            }
            var artist = _artistManager.GetOne(filter: x => x.Id == CurrentLoggedUserId, includeProperties: $"{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            var model = new PagingModel<ApplicationUser>()
            {
                PagingList = artist.Follows.Where(x => x.Follower.Name.ToLower().Contains(searchValue.Trim().ToLower())).Select(x => x.Follower).ToList()
            };
            return View("Followers", model);
        }
        [HttpGet]
        public IActionResult GetNextFollowers([FromQuery] PagingModel<ApplicationUser> model)
        {
            if (string.IsNullOrWhiteSpace(model.SearchQuery))
                model.SearchQuery = string.Empty;
            var artist = _artistManager.GetOne(x => x.Id == CurrentLoggedUserId,$"{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            model.PagingList = artist.Follows.Where(x => x.Follower.Name.ToLower().Contains(model.SearchQuery.Trim().ToLower())).Skip(model.Skip).Take(model.Take).Select(x => x.Follower).ToList();
            return View(model);
        }
    }
}