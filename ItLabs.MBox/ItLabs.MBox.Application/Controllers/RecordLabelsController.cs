using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Common.Extensions;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelsController : BaseController
    {
        private readonly IArtistManager _artistsManager;
        private readonly IS3Manager _s3Manager;
        private readonly IEmailsManager _emailsManager;
        private readonly IRecordLabelManager _recordLabelManager;

        public RecordLabelsController(IArtistManager artistsManager, UserManager<ApplicationUser> userManager, IEmailsManager emailsManager, IRecordLabelManager recordLabelManager, IS3Manager s3Manager) : base(userManager)
        {
            _artistsManager = artistsManager;
            _emailsManager = emailsManager;
            _recordLabelManager = recordLabelManager;
            _s3Manager = s3Manager;
        }

        public IActionResult Index()
        {
            PagingModel<Artist> model = new PagingModel<Artist>();
            model.PagingList = _artistsManager.GetRecordLabelArtists(CurrentLoggedUserId, model.Skip, model.Take, string.Empty);
            return View(model);

        }

        [HttpGet]
        public IActionResult GetRecordLabelArtists([FromQuery] PagingModel<Artist> model)
        {
            model.PagingList = _artistsManager.GetRecordLabelArtists(CurrentLoggedUserId, model.Skip, model.Take, model.SearchQuery).ToList();
            return View("NextArtists", model);
        }

        [HttpGet]
        public IActionResult AddNewArtist()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewArtist(InviteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var recordLabel = _recordLabelManager.GetOne(filter: x => x.Id == CurrentLoggedUserId, includeProperties: $"{nameof(User)}");
            if (recordLabel == null)
                return View(model);

            if (_recordLabelManager.GetNumberOfArtists(CurrentLoggedUserId) >= MBoxConstants.MaximumArtistsAllowed)
            {
                ModelState.AddModelError("Email", "Artist limit (50) reached. Cannot add new artist.");
                return View(model);
            }

            var response = _userManager.CreateUser(model.Name, model.Email, Role.Artist);

            if (response == null)
            {
                ModelState.AddModelError("EMail", "Email already exists");
                return View(model);
            }
            var user = response.Result;
            _artistsManager.Create(new Artist { User = user, RecordLabelArtists = new List<RecordLabelArtist> { new RecordLabelArtist { RecordLabel = recordLabel } } }, CurrentLoggedUserId);
            _artistsManager.Save();
            var code = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
            _emailsManager.PrepareSendMail(EmailTemplateType.InvitedArtist, model.Email, callbackUrl);

            return View("SuccessfullyInvitedArtist");

        }
        [HttpGet]
        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Artists";
            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            PagingModel<Artist> model = new PagingModel<Artist>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                model.PagingList = _artistsManager.GetRecordLabelArtists(CurrentLoggedUserId, model.Skip, model.Take, search);
                return View("Index", model);
            }
            return RedirectToAction("Index", "RecordLabels");
        }


        [HttpGet]
        public IActionResult AddMultipleArtists()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddMultipleArtists(List<IFormFile> files)
        {
            if (!files.Any())
            {
                return View(new List<string>() { "Please choose a file" });
            }

            var response = _recordLabelManager.ValidateCsvFile(files[0], CurrentLoggedUserId);

            if (response.Errors.Count > 0)
            {
                return View(response.Errors);
            }
            if (_recordLabelManager.GetNumberOfArtists(CurrentLoggedUserId) + response.UsersToBeAdded.Count > MBoxConstants.MaximumArtistsAllowed)
            {
                response.Errors.Add("Artist Limit (50) exceeded");
                return View(response.Errors);
            }

            var artistList = _recordLabelManager.CreateMultipleArtists(response.UsersToBeAdded, CurrentLoggedUserId);

            if (artistList == null)
            {
                response.Errors.Add("An error occured while adding artists, try again!");
                return View(response.Errors);
            }

            if (artistList.Count != 0)
                SendActivationMails(artistList);

            return View("SuccessfullyAddedMultiple");
        }
        private void SendActivationMails(IList<Artist> artists)
        {
            //prepare the emails set the dto
            IList<MailDto> mailingList = new List<MailDto>();
            var template = _emailsManager.GetOne(filter: c => c.Type == EmailTemplateType.InvitedArtist);

            foreach (var artist in artists)
            {
                var mailDto = new MailDto();
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
                _emailsManager.SendMultipleMails(mailingList, configuration);
            }).Start();
            //fire and forget
        }

        [HttpPost]
        public IActionResult DeleteArtist(int artistlId)
        {
            _artistsManager.DeleteArtist(CurrentLoggedUserId, artistlId);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ChangePicture(List<IFormFile> uploadedFiles)
        {
            var model = new MyAccountViewModel();
            var imageS3Name = string.Empty;
            if (uploadedFiles.Count == 0)
            {
                ModelState.AddModelError("Picture", "Please choose a picture!");
                return View("MyAccount", model);
            }

            var formFile = uploadedFiles[0];
            if (formFile.Length > MBoxConstants.MaximumImageSizeAllowed)
            {
                ModelState.AddModelError("Picture", "Maximum 3MB picture size allowed!");
                return View("MyAccount", model);
            }

            imageS3Name = _s3Manager.UploadFile(formFile);
            var currentUser = _userManager.FindByIdAsync(CurrentLoggedUserId.ToString()).Result;
            currentUser.Picture = imageS3Name;

            _userManager.UpdateAsync(currentUser).Wait();
            return RedirectToAction("MyAccount");
        }
    }
}
