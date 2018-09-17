using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Domain.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelsController : Controller
    {
        private ArtistManager _artistsManager;
        private readonly MBoxUserManager _userManager;
        private readonly IEmailsManager _emailsManager;
        RecordLabelManager _recordLabelManager;

        public RecordLabelsController(ArtistManager artistsManager, MBoxUserManager userManager,IEmailsManager emailsManager, RecordLabelManager recordLabelManager)
        {      
            _artistsManager = artistsManager;
            _userManager = userManager;
            _emailsManager = emailsManager;
            _recordLabelManager = recordLabelManager;
        }

        public IActionResult Index()
        {
           var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));         

           DashboardViewModel model = new DashboardViewModel() { RecordLabelId = recordLabelId, Skip = 0, Take = 20 };
           model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetRecordLabelArtists([FromQuery] DashboardViewModel model)
        {
            var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));

            model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take).ToList();

            return View("NextArtists", model);
        }

        [HttpGet]
        public IActionResult AddNewArtist()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewArtistAsync(InviteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var recordLabelId = Convert.ToInt32(_userManager.GetUserId(HttpContext.User));
            var recordLabel = _recordLabelManager.GetOne(filter: x=>x.Id == recordLabelId, includeProperties: $"{nameof(User)}");

            var user = _userManager.CreateUser(model.Name, model.Email, Role.Artist).Result;

            if (user == null)
            {
                ModelState.AddModelError("EMail", "Email already exists");
                return View("AddNewArtist", model);
            }

             _artistsManager.Create(new Artist { User= user }, recordLabelId);
            _artistsManager.Save();
            var artist = _artistsManager.GetOne(filter: x => x.User == user, includeProperties:$"{nameof(User)}");
            _artistsManager.AddArtistToRecordLabel(artist, recordLabel);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(artist.Id.ToString(), code, Request.Scheme);
            await _emailsManager.SendMail(EmailTemplateType.InvitedArtist, model.Email, callbackUrl);

            return View("SuccessfullyInvitedArtist");

        }



        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Artists";
            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));

            DashboardViewModel model = new DashboardViewModel() { RecordLabelId = recordLabelId, Skip = 0, Take = 20 };

            if (search != null)
            {
                model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, search);
                return View("Index", model);
            }

            model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);
            return RedirectToAction("Index", "RecordLabels");
        }

    }
}