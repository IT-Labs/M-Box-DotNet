using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : BaseController
    {
        private ISongManager _songsManager;
        private IArtistManager _artistsManager;
        private IRecordLabelManager _recordLabelManager;
        private IEmailsManager _emailManager;
        public HomeController(ISongManager songsManager, IArtistManager artistsManager, IRecordLabelManager recordLabelManager, IEmailsManager emailManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;

        }

        public IActionResult Index()
        {
            if (!HasAccess())
                return RedirectProperly();

            ViewData["Message"] = "Home";
            HomeViewModel model = new HomeViewModel
            {
                RecentlyAddedSongs = _songsManager.GetRecentlyAddedSongs(MBoxConstants.HomeItemsToDisplay),
                MostFollowedArtists = _artistsManager.GetMostFollowedArtists(MBoxConstants.HomeItemsToDisplay),
                MostPopularArtistSongs = _songsManager.GetMostPopularArtistSongs(MBoxConstants.HomeItemsToDisplay)
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult About()
        {
            if (!HasAccess())
                return RedirectProperly();

            ViewData["Message"] = "About page";
            AboutViewModel model = new AboutViewModel
            {
                WeCooperateWith = _recordLabelManager.GetAll(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult About(AboutViewModel model)
        {
            model.WeCooperateWith = _recordLabelManager.GetAll(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();

            ViewData["Message"] = "About page";
            if (ModelState.IsValid)
            {
                _emailManager.PrepareContactFormMail(model.Name, model.Email, model.Message);
                ModelState.AddModelError("Message", "Message successfully sent");
                return RedirectToAction("About", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Artists()
        {
            if (!HasAccess())
                return RedirectProperly();

            ViewData["Message"] = "Artists";

            var model = new PagingModel<Artist>();
            model.PagingList = _artistsManager.Get(
                includeProperties: $"{nameof(Artist.User)}," +
                        $"{nameof(Artist.RecordLabelArtists)}.{nameof(RecordLabel)}.{nameof(RecordLabel.User)}",
                skip: model.Skip,
                take: model.Take).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextArtists([FromQuery] PagingModel<Artist> model)
        {
            model.PagingList = _artistsManager.Get(
                includeProperties: $"{nameof(Artist.User)}," +
                        $"{nameof(Artist.RecordLabelArtists)}.{nameof(RecordLabel)}.{nameof(RecordLabel.User)}",
                skip: model.Skip,
                take: model.Take).ToList();

            return View("GetNextArtists", model);
        }

        [HttpGet]
        public IActionResult RecordLabels()
        {
            if (!HasAccess())
                return RedirectProperly();

            ViewData["Message"] = "RecordLabels";

            var model = new PagingModel<RecordLabel>();
            model.PagingList = _recordLabelManager.Get(skip: model.Skip, take: model.Take, includeProperties: $"{nameof(RecordLabel.User)}").ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            ViewData["Message"] = "RecordLabels";
            model.PagingList = _recordLabelManager.Get(skip: model.Skip, take: model.Take, includeProperties: $"{nameof(RecordLabel.User)}").ToList();
            return View("NextRecordLabels", model);
        }

        [HttpPost]
        public IActionResult SongDetails(int songId)
        {
            var songDetails = new HomeViewModel() { };
            songDetails.SongDetails = _songsManager.GetOne(
                filter: x => x.Id == songId,
                includeProperties: $"{nameof(Song.Artist)}.{nameof(Artist.User)}");

            return View(songDetails);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UploadFile()
        {

            return View();
        }
        private bool HasAccess()
        {
            return !((HttpContext.User.IsInRole(nameof(Role.SuperAdmin))) || (HttpContext.User.IsInRole(nameof(Role.RecordLabel))));
        }
        private IActionResult RedirectProperly()
        {
            if (HttpContext.User.IsInRole(nameof(Role.SuperAdmin)))
                return RedirectToAction("Index", "Admins");
            if (HttpContext.User.IsInRole(nameof(Role.RecordLabel)))
                return RedirectToAction("Index", "RecordLabels");
            return null;
        }
        [HttpPost]
        public IActionResult ArtistDetails(int artistId)
        {
            var artist = _artistsManager.GetOne(x => x.Id == artistId,includeProperties:$"{nameof(Artist.User)},{nameof(Artist.Songs)}");
            return View(artist);
        }
    }
}
