using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts;
using Microsoft.AspNetCore.Authorization;
using ItLabs.MBox.Contracts.Enums;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : Controller
    {
        private ISongManager _songsManager;
        private IArtistManager _artistsManager;
        private IRecordLabelManager _recordLabelManager;
        private IEmailsManager _emailManager;
        public HomeController(ISongManager songsManager, IArtistManager artistsManager, IRecordLabelManager recordLabelManager, IEmailsManager emailManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;
        }

        public IActionResult Index()
        {
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
            ViewData["Message"] = "About page";
            AboutViewModel model = new AboutViewModel{WeCooperateWith = _recordLabelManager.GetAllRecordLabels()};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult About(AboutViewModel model)
        {
            ViewData["Message"] = "About page";
            if (ModelState.IsValid)
            {
                _emailManager.SentContactFormMail(model.Name, model.Email, model.Message);
            }
            model.WeCooperateWith = _recordLabelManager.GetAllRecordLabels();
            return View(model);
        }


        public IActionResult Artists()
        {
            ViewData["Message"] = "Artists";

            return View();
        }
        [HttpGet]
        public IActionResult RecordLabels()
        {
            ViewData["Message"] = "RecordLabels";
            var model = new PagingModel<RecordLabel>() { Skip = 0, Take = 25 };
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            ViewData["Message"] = "RecordLabels";
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take).ToList() ;
            return View("NextRecordLabels", model);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
