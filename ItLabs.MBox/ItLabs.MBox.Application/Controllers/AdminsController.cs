using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Application.Models.AdminViewModels;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using ItLabs.MBox.Application.Models;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminsController : Controller
    {
        private IRecordLabelManager _recordLabelManager;
        private readonly MBoxUserManager _userManager;
        private readonly IEmailsManager _emailManager;

        public AdminsController(IRecordLabelManager recordLabelManager, MBoxUserManager userManager, IEmailsManager emailManager)
        {
            _recordLabelManager = recordLabelManager;
            _userManager = userManager;
            _emailManager = emailManager;
        }

        [HttpGet]
        public IActionResult Index()
        {   
              var model = new PagingModel<RecordLabel>() { Skip = 0, Take = 20 };
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take).ToList();
            return View("NextRecordLabels", model);
        }

        [HttpGet]
        public IActionResult AddNewRecordLabel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRecordLabelAsync(AddNewRecordLabelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userManager.CreateUser(model.Name, model.Email, Role.RecordLabel).Result;
            if(user == null)
            {
                return View(model);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
            await _emailManager.SendMail(EmailTemplateType.InvitedRecordLabel, model.Email, callbackUrl);

            return View("SuccessfullyInvited");

        }
        [HttpGet]
        public IActionResult Search([FromQuery]string query)
        {
            var model = new PagingModel<RecordLabel>();
            return View();
        }

    }
}