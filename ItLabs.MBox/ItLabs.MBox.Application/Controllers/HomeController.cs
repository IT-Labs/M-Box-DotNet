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
        private IRepository _repository;
        public HomeController(IRepository repository, ISongManager songsManager, IArtistManager artistsManager, IRecordLabelManager recordLabelManager, IEmailsManager emailManager, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _songsManager = songsManager;
            _artistsManager = artistsManager;
            _recordLabelManager = recordLabelManager;
            _emailManager = emailManager;
            _repository = repository;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.IsInRole(nameof(Role.SuperAdmin)))
                return RedirectToAction("Index", "Admins");

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
            if (HttpContext.User.IsInRole(nameof(Role.SuperAdmin)))
                return RedirectToAction("Index", "Admins");

            ViewData["Message"] = "About page";
            AboutViewModel model = new AboutViewModel
            {
                WeCooperateWith = _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult About(AboutViewModel model)
        {
            model.WeCooperateWith = _repository.GetAll<RecordLabel>(
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


        public IActionResult Artists()
        {
            ViewData["Message"] = "Artists";

            return View();
        }
        [HttpGet]
        public IActionResult RecordLabels()
        {
            if (HttpContext.User.IsInRole(nameof(Role.SuperAdmin)))
                return RedirectToAction("Index", "Admins");

            ViewData["Message"] = "RecordLabels";
            var model = new PagingModel<RecordLabel>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeHomeLists };
            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            ViewData["Message"] = "RecordLabels";
            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            return View("NextRecordLabels", model);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UploadFile()
        {

            return View();
        }
        
    }
}
