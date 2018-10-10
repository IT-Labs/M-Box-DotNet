using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.ArtistsViewModel;
using ItLabs.MBox.Application.Models.HomeViewModels;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Data_Structures;
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
        private ISearchManager _searchManager;
        private IEmailsManager _emailManager;
        public HomeController(ISearchManager searchManager, ILogger<HomeController> logger, ISongManager songsManager, IArtistManager artistsManager, IRecordLabelManager recordLabelManager, IEmailsManager emailManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;
            _logger = logger;
            _searchManager = searchManager;
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

            ViewData["Message"] = "About";
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
            ViewData["Message"] = "About";

            model.WeCooperateWith = _recordLabelManager.GetAll(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();

            if (ModelState.IsValid)
            {
                _emailManager.PrepareContactFormMail(model.Name, model.Email, model.Message);
                //TempData["successMessage"] = "Message successfully sent";
                
                ViewBag.MailSend = "Message successfully sent";

                ModelState.Clear();

                model.Email = string.Empty;
                model.Message = string.Empty;
                model.Name = string.Empty;

                return View(model);
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
            return View(model);
        }

        public IActionResult SongDetails(int Id)
        {
            var songDetails = new HomeViewModel() { };
            songDetails.SongDetails = _songsManager.GetOne(
                filter: x => x.Id == Id,
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
        
        public IActionResult ArtistDetails(int Id)
        {
            var model = new ArtistDetailsViewModel();
            model.Artist = _artistsManager.GetOne(x => x.Id == Id, includeProperties: $"{nameof(Artist.User)},{nameof(Artist.Songs)},{nameof(Artist.Follows)}.{nameof(Follow.Follower)}");
            model.PagingModelSongs = new PagingModel<Song>() { PagingList = model.Artist.Songs.Take(MBoxConstants.initialTakeHomeLists).ToList() };
            model.CurrentLoggedUserId = CurrentLoggedUserId;
            model.FollowingCount = _userManager.Users.Where(x => x.Id == Id).Include($"{nameof(Artist.Follows)}.{nameof(Follow.Artist)}").FirstOrDefault().Follows.Select(x => x.Artist == model.Artist).ToList().Count;
            model.FollowersCount = model.Artist.Follows.Select(x => x.Follower).ToList().Count;
            return View(model);
        }
        
        public IActionResult RecordLabelDetails(int Id)
        {
            var model = new RecordLabelDetailsViewModel();
            model.RecordLabel = _recordLabelManager.GetOne(filter: x => x.Id == Id, includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}.{nameof(Artist)}.{nameof(Artist.User)}");
            model.PagingModelArtists = new PagingModel<Artist>();
            model.PagingModelArtists.PagingList = model.RecordLabel.RecordLabelArtists.Select(x => x.Artist).Take(MBoxConstants.initialTakeHomeLists).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextSongs([FromQuery] int Id, [FromQuery] int skip, [FromQuery]  int take)
        {
            var model = new PagingModel<Song>
            {
                PagingList = _songsManager.Get(filter: x => x.Artist.Id == Id,
                skip: skip,
                take: take,
                includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}").ToList()
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult GetNextRecordLabelArtists([FromQuery] int Id, [FromQuery] int skip, [FromQuery]  int take)
        {
            var recordLabel = _recordLabelManager.Get(filter: x => x.Id == Id,
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
                _artistsManager.ToggleFollow(artistId, CurrentLoggedUserId);
                return Ok();
            }
            catch (Exception ex)
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
        public IActionResult MainSearch(string searchValue, string showby = nameof(SearchType.MostRelevant))
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return View(new SearchResultsViewModel()
                {
                    SearchValue = string.Empty,
                    SearchType = SearchType.MostRelevant
                });
            }
            var model = new SearchResultsViewModel()
            {
                SearchValue = searchValue.ToLower(),
                SearchType = SearchType.MostRelevant
            };
            var allResults = new PriorityQueue<object>();
            switch (showby)
            {
                case nameof(SearchType.MostRelevant):
                    allResults = _searchManager.Search(searchValue.Trim().ToLower(), SearchType.MostRelevant);
                    model.SearchType = SearchType.MostRelevant;
                    break;
                case nameof(SearchType.SongName):
                    allResults = _searchManager.Search(searchValue.Trim().ToLower(), SearchType.SongName);
                    model.SearchType = SearchType.SongName;
                    break;
                case nameof(SearchType.Lyrics):
                    allResults = _searchManager.Search(searchValue.Trim().ToLower(), SearchType.Lyrics);
                    model.SearchType = SearchType.Lyrics;
                    break;
                case nameof(SearchType.ArtistName):
                    allResults = _searchManager.Search(searchValue.Trim().ToLower(), SearchType.ArtistName);
                    model.SearchType = SearchType.ArtistName;
                    break;
                case nameof(SearchType.RecordLabelName):
                    allResults = _searchManager.Search(searchValue.Trim().ToLower(), SearchType.RecordLabelName);
                    model.SearchType = SearchType.RecordLabelName;
                    break;
            }
            int toTake = 0;
            if (allResults.Count > MBoxConstants.initialTakeHomeLists)
                toTake = MBoxConstants.initialTakeHomeLists;
            else
                toTake = allResults.Count;

            for (int i = 0; i < toTake; i++)
            {
                model.Results.Enqueue(allResults.Peek(), allResults.PeekPriority());
                allResults.Dequeue();
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetNextSearchResults([FromQuery]SearchResultsViewModel model)
        {
            var allResults = _searchManager.Search(model.SearchValue.Trim().ToLower(), model.SearchType);
            int toTake = 0;
            if (allResults.Count > model.Take + model.Skip)
                toTake = model.Take;
            else
                toTake = allResults.Count - model.Skip;
            if (toTake > 0)
            {
                for (int i = 0; i < model.Skip; i++)
                {
                    allResults.Dequeue();
                }
            }
           

            for (int i = 0 ; i < toTake; i++)
            {
                model.Results.Enqueue(allResults.Peek(), allResults.PeekPriority());
                allResults.Dequeue();
            }

            return View(model);
        }
    }

}

