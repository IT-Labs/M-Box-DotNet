using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Common.Extentions;
using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            DashboardViewModel model = new DashboardViewModel() { RecordLabelId = CurrentLoggedUser, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, string.Empty);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetRecordLabelArtists([FromQuery] DashboardViewModel model)
        {
            model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, string.Empty).ToList();
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

            var response = _userManager.CreateUser(model.Name, model.Email, Role.Artist);

            if (response == null)
            {
                ModelState.AddModelError("EMail", "Email already exists");
                return View("AddNewArtist", model);
            }
            var user = response.Result;
            _artistsManager.Create(new Artist { User = user }, CurrentLoggedUser);
            _artistsManager.Save();
            var artist = _artistsManager.GetOne(filter: x => x.User == user, includeProperties: $"{nameof(User)}");
            _artistsManager.AddArtistToRecordLabel(artist, recordLabel);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(artist.Id.ToString(), code, Request.Scheme);
            _emailsManager.PerpareSendMail(EmailTemplateType.InvitedArtist, model.Email, callbackUrl);

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

            DashboardViewModel model = new DashboardViewModel() { RecordLabelId = CurrentLoggedUser, Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };

            if (search != null)
            {
                model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, search);
                return View("Index", model);
            }

            model.PagingList = _artistsManager.GetSearchedArtists(model.RecordLabelId, model.Skip, model.Take, string.Empty);
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
                sendActivationMails(response.ArtistsToBeAdded);
            //TODO: return number of created
            return View("SuccessfullyAddedMultiple");
        }
        private void sendActivationMails(IList<Artist> artists)
        {
            //prepare the emails set the dto
            IList<MailDto> mailingList = new List<MailDto>();
            var template = _emailsManager.GetOne(filter: c => c.Type == EmailTemplateType.InvitedArtist);
            var mailDto = new MailDto();
            foreach (var artist in artists)
            {
                mailDto.EmailAddress = artist.User.Email;
                mailDto.Subject = template.Subject;
                mailDto.Body = template.Body.Replace("[Name]", artist.User.Name);
                var code = _userManager.GeneratePasswordResetTokenAsync(artist.User).Result;
                var callbackUrl = Url.ResetPasswordCallbackLink(artist.User.Id.ToString(), code, Request.Scheme);
                mailDto.Body = mailDto.Body.Replace("[Link]", "<a href=" + callbackUrl + ">" + template.LinkText + "</a>");
                mailingList.Add(mailDto);
            }
            var configuration = _emailsManager.GetConfiguration();
            //fire and forget
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                _emailsManager.SendMultipleMailSmtp(mailingList, configuration);
            }).Start();
            //fire and forget
        }
    }
}