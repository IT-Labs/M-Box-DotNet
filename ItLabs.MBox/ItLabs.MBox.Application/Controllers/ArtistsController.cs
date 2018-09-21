using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly IRepository _repository;
        private readonly ISongManager _songManager;
        public ArtistsController(IRepository repository, ISongManager songManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songManager = songManager;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var model = new PagingModel<Song>() { ArtistlId = CurrentLoggedUserId, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _songManager.GetArtistSongs(model.ArtistlId, model.Skip, model.Take, string.Empty);

            return View(model);
        }

        public IActionResult Search(string searchValue)
        {
            var model = new PagingModel<Song>() { ArtistlId = CurrentLoggedUserId, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };

            if (searchValue != null)
            {
                model.PagingList = _songManager.GetArtistSongs(model.ArtistlId, model.Skip, model.Take, searchValue);
                return View("Index", model);
            }

            model.PagingList = _songManager.GetArtistSongs(model.ArtistlId, model.Skip, model.Take, string.Empty);
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
        public IActionResult AddNewSong(AddNewSongViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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