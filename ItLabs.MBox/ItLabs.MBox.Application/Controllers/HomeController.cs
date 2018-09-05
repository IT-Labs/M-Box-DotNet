using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : Controller
    {
        private ISongsManager _songsManager;
        private IArtistsManager _artistsManager;
        private IRecordLabelsManager _recordLabelManager;
        private IEmailManager _emailManager;
        public HomeController(ISongsManager songsManager, IArtistsManager artistsManager, IRecordLabelsManager recordLabelManager, IEmailManager emailManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;
        }
        public IActionResult Index()
        {
            ViewData["Message"] = "Home";
            HomeViewModel model = new HomeViewModel();
            model.RecentlyAddedSongs = _songsManager.GetRecentlyAddedSongs(5);
            model.MostFollowedArtists = _artistsManager.GetMostFollowedArtists(5);
            model.RecentlyAddedSongsOfMostPopularArtist = _songsManager.GetRecentlyAddedSongsOfMostPopularArtists(5);
            return View(model);
        }
        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "About page";
            AboutViewModel model = new AboutViewModel();
            model.WeCooperateWith = _recordLabelManager.GetAllRecordLabels();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult About(ContactFormViewModel model)
        {
            ViewData["Message"] = "About page";
            if (ModelState.IsValid)
            {
                //_emailManager.SendMail(,model.Email)
            }
            return View(model);
        }


        public IActionResult Artists()
        {
            ViewData["Message"] = "Artists";

            return View();
        }

        public IActionResult RecordLabels()
        {
            ViewData["Message"] = "RecordLabels";
            RecordLabelViewModel model = new RecordLabelViewModel();
            model.RecordLabels = _recordLabelManager.GetAllRecordLabels();
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
