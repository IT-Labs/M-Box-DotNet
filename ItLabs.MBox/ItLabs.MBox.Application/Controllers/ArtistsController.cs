using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : BaseController
    {
        private readonly ISongManager _songManager;
        public ArtistsController(ISongManager songManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songManager = songManager;
        }
        public IActionResult Index()
        {
            var model = new PagingModel<Song>() { ArtistlId = CurrentLoggedUser, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _songManager.GetArtistSongs(model.ArtistlId, model.Skip, model.Take, string.Empty);

            return View(model);
        }

        public IActionResult Search(string searchValue)
        {
            var model = new PagingModel<Song>() { ArtistlId = CurrentLoggedUser, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };

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
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}