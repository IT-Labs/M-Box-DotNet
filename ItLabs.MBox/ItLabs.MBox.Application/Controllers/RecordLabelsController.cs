using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Common.Extentions;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelsController : BaseController
    {
        private IArtistManager _artistsManager;
       
        private readonly IEmailsManager _emailsManager;
        private readonly IRecordLabelManager _recordLabelManager;

        public RecordLabelsController(IArtistManager artistsManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager, IRecordLabelManager recordLabelManager): base(userManager)
        {
            _artistsManager = artistsManager;
            _emailsManager = emailsManager;
            _recordLabelManager = recordLabelManager;
            
        }

        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel() { RecordLabelId = CurrentLoggedUser, Skip = 0, Take = 20 };
            model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetRecordLabelArtists([FromQuery] DashboardViewModel model)
        {
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
            var recordLabel = _recordLabelManager.GetOne(filter: x => x.Id == CurrentLoggedUser, includeProperties: $"{nameof(User)}");
            if (recordLabel == null)
                return View("AddNewArtist", model);

            var user = _userManager.CreateUser(model.Name, model.Email, Role.Artist).Result;

            if (user == null)
            {
                ModelState.AddModelError("EMail", "Email already exists");
                return View("AddNewArtist", model);
            }

            _artistsManager.Create(new Artist { User = user }, CurrentLoggedUser);
            _artistsManager.Save();
            var artist = _artistsManager.GetOne(filter: x => x.User == user, includeProperties: $"{nameof(User)}");
            _artistsManager.AddArtistToRecordLabel(artist, recordLabel);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(artist.Id.ToString(), code, Request.Scheme);
            await _emailsManager.PerpareSendMail(EmailTemplateType.InvitedArtist, model.Email, callbackUrl);

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

            DashboardViewModel model = new DashboardViewModel() { RecordLabelId = CurrentLoggedUser, Skip = 0, Take = 20 };

            if (search != null)
            {
                model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, search);
                return View("Index", model);
            }

            model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);
            return RedirectToAction("Index", "RecordLabels");
        }


        [HttpGet]
        public IActionResult AddMultipleArtists()
        {
            return View();
        }


        [HttpPost]
        public IActionResult UploadCsvFile(List<IFormFile> files)
        {
            var response = _recordLabelManager.ValidateCsvFile(files[0], CurrentLoggedUser);
            if(response.Errors.Count != 0)
            {
                return View("AddMultipleArtists", response);
            }
            var numberOfCreated = _recordLabelManager.CreateMultipleArtists(response.ArtistsToBeAdded,CurrentLoggedUser);
            if(numberOfCreated != 0)
                Task.Run(() => SendMails(response.ArtistsToBeAdded));
            //return number of created
            return View("SuccessfullyAddedMultiple");
        }
        private void SendMails(IList<Artist> artistsToBeAdded)
        {
            foreach (var artist in artistsToBeAdded)
            {
                var code = _userManager.GeneratePasswordResetTokenAsync(artist.User).Result;
                var callbackUrl = Url.ResetPasswordCallbackLink(artist.User.Id.ToString(), code, Request.Scheme);
                _emailsManager.PerpareSendMail(EmailTemplateType.InvitedArtist, artist.User.Email, callbackUrl);
            }
        }
    }
}