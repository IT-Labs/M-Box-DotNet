using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : Controller
    {
        private ISongsManager _songsManager;
        private IArtistsManager _artistsManager;
        public HomeController(ISongsManager songsManager, IArtistsManager artistsManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
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

        public IActionResult About()
        {
            ViewData["Message"] = "About page";

            return View();
        }

        public IActionResult Artists()
        {
            ViewData["Message"] = "Artists";

            return View();
        }

        public IActionResult RecordLabels()
        {
            ViewData["Message"] = "RecordLabels";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
