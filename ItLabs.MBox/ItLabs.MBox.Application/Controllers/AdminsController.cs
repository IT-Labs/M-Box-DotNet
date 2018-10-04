using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Common.Extensions;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminsController : BaseController
    {
        private IRecordLabelManager _recordLabelManager;
        private readonly IEmailsManager _emailsManager;
        private ILogger<AdminsController> _logger;

        public AdminsController(ILogger<AdminsController> logger,IRecordLabelManager recordLabelManager, UserManager<ApplicationUser> userManager, IEmailsManager emailManager) : base(userManager)
        {
            _recordLabelManager = recordLabelManager;
            _emailsManager = emailManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new PagingModel<RecordLabel>();
            model.PagingList = _recordLabelManager.Get(skip: model.Skip, take: model.Take, includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            model.PagingList = _recordLabelManager.SearchRecordLabels(model.SearchQuery, model.Skip, model.Take).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult AddNewRecordLabel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewRecordLabel(InviteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var response = _userManager.CreateUser(model.Name, model.Email, Role.RecordLabel);
            if (response == null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View( model);
            }
            var user = response.Result;
            _recordLabelManager.Create(new RecordLabel() { User = user }, CurrentLoggedUserId);
            _recordLabelManager.Save();
            var code =  _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
            _emailsManager.PrepareSendMail(EmailTemplateType.InvitedRecordLabel, model.Email, callbackUrl);

            return View("SuccessfullyInvited");

        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            var model = new PagingModel<RecordLabel>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                model.PagingList = _recordLabelManager.SearchRecordLabels(search, model.Skip, model.Take);
                return View("Index", model);
            }

            return RedirectToAction("Index", "Admins");
        }
        [HttpPost]
        public IActionResult DeleteRecordLabel(int recordLabelId)
        {
            var user = _userManager.FindByIdAsync(recordLabelId.ToString()).Result;
            if (user == null)
                return RedirectToAction("Index");
 
            _recordLabelManager.DeleteRecordLabel(user);
            return RedirectToAction("Index");
        }
    }
}