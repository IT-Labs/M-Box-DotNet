﻿using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : BaseController
    {
        private ISongManager _songsManager;
        private IArtistManager _artistsManager;
        private IRecordLabelManager _recordLabelManager;
        private ILogger<HomeController> _logger;
        private IEmailsManager _emailManager;
        public HomeController(ILogger<HomeController> logger, ISongManager songsManager, IArtistManager artistsManager, IRecordLabelManager recordLabelManager, IEmailsManager emailManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;
            _logger = logger;
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
            var model = new ArtistDetailsViewModel();
            model.Artist = _artistsManager.GetOne(x => x.Id == artistId, includeProperties: $"{nameof(Artist.User)},{nameof(Artist.Songs)},{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            model.PagingModelSongs = new PagingModel<Song>() { PagingList = model.Artist.Songs.Take(MBoxConstants.initialTakeHomeLists).ToList() };
            model.CurrentLoggedUserId = CurrentLoggedUserId;
            model.FollowingCount = _userManager.Users.Where(x => x.Id == artistId).Include($"{nameof(Artist.Follows)}.{nameof(Follow.Artist)}").FirstOrDefault().Follows.Select(x=>x.Artist == model.Artist).ToList().Count;
            model.FollowersCount = model.Artist.Follows.Select(x => x.Follower).ToList().Count;
            return View(model);
        }
        [HttpPost]
        public IActionResult RecordLabelDetails(int recordLabelId)
        {
            var model = new RecordLabelDetailsViewModel();
            model.RecordLabel = _recordLabelManager.GetOne(filter: x => x.Id == recordLabelId, includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}.{nameof(Artist)}.{nameof(Artist.User)}");
            model.PagingModelArtists = new PagingModel<Artist>();
            model.PagingModelArtists.PagingList = model.RecordLabel.RecordLabelArtists.Select(x => x.Artist).Take(MBoxConstants.initialTakeHomeLists).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextSongs([FromQuery] int artistId, [FromQuery] int skip, [FromQuery]  int take)
        {
            var model = new PagingModel<Song>
            {
                PagingList = _songsManager.Get(filter: x => x.Artist.Id == artistId,
                skip: skip,
                take: take,
                includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList()
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult GetNextRecordLabelArtists([FromQuery] int recordLabelId, [FromQuery] int skip, [FromQuery]  int take)
        {
            var recordLabel = _recordLabelManager.Get(filter: x => x.Id == recordLabelId,
                includeProperties: $"{nameof(Artist.User)}," +
                        $"{nameof(Artist.RecordLabelArtists)}.{nameof(Artist)}.{nameof(Artist.User)}").FirstOrDefault();
            var model = new PagingModel<Artist>()
            {
                PagingList = recordLabel.RecordLabelArtists.Select(x => x.Artist).Skip(skip).Take(take).ToList(),
                Skip = skip,
                Take = take
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult ToggleFollow([FromQuery] int artistId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Error();
            }
            try
            {
                _artistsManager.ToggleFollow(artistId,CurrentLoggedUserId);
                return Ok();
            }catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return Error();
            }
        }
        [HttpGet]
        public IActionResult Following()
        {
            var model = new PagingModel<Artist>();
            var user = _userManager.Users.Where(x => x.Id == CurrentLoggedUserId).Include($"{nameof(Artist.Follows)}.{nameof(Follow.Artist)}.{nameof(Artist.User)}").FirstOrDefault();
            model.PagingList = user.Follows.Select(x => x.Artist).Skip(model.Skip).Take(model.Take).ToList();
            return View(model);
        }
        public IActionResult SearchFollowing(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return RedirectToAction("Following");
            }
            var user = _userManager.Users.Where(x => x.Id == CurrentLoggedUserId).Include($"{ nameof(Artist.Follows)}.{ nameof(Follow.Artist)}.{nameof(Artist.User)}").FirstOrDefault();
            var model = new PagingModel<Artist>()
            {
                PagingList = user.Follows.Where(x => x.Artist.User.Name.ToLower().Contains(searchValue.Trim().ToLower())).Select(x => x.Artist).ToList()
            };
            return View("Following", model);
        }
        [HttpGet]
        public IActionResult GetNextFollowing([FromQuery] PagingModel<Artist> model)
        {
            if (string.IsNullOrWhiteSpace(model.SearchQuery))
                model.SearchQuery = string.Empty;
            var user = _userManager.Users.Where(x => x.Id == CurrentLoggedUserId).Include($"{nameof(Artist.Follows)}.{nameof(Follow.Artist)}.{nameof(Artist.User)}").FirstOrDefault();
            model.PagingList = user.Follows.Where(x => x.Artist.User.Name.ToLower().Contains(model.SearchQuery.Trim().ToLower())).Skip(model.Skip).Take(model.Take).Select(x => x.Artist).ToList();
            return View(model);
        }

    }
}
