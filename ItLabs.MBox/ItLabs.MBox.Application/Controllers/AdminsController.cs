using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Common.Extentions;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminsController : BaseController
    {
        private IRecordLabelManager _recordLabelManager;
        private readonly IEmailsManager _emailsManager;


        public AdminsController(IRecordLabelManager recordLabelManager, UserManager<ApplicationUser> userManager, IEmailsManager emailManager):base(userManager)
        {
            _recordLabelManager = recordLabelManager;
            _emailsManager = emailManager;
        }

        [HttpGet]
        public IActionResult Index()
        {   
            var model = new PagingModel<RecordLabel>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            return View("NextRecordLabels", model);
        }

        [HttpGet]
        public IActionResult AddNewRecordLabel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRecordLabelAsync(InviteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userManager.CreateUser(model.Name, model.Email, Role.RecordLabel).Result;
            if(user == null)
            {
                ModelState.AddModelError("EMail", "Email already exists");
                return View("AddNewRecordLabel",model);
            }
            _recordLabelManager.Create(new RecordLabel() {User=user },CurrentLoggedUser);
            _recordLabelManager.Save();
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
            await _emailsManager.PerpareSendMail(EmailTemplateType.InvitedRecordLabel, model.Email, callbackUrl);

            return View("SuccessfullyInvited");

        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            var model = new PagingModel<RecordLabel>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };

            if (search != null)
            {
                model.PagingList = _recordLabelManager.GetSearchedRecordLabels(search, model.Skip, model.Take);
                return View("Index", model);
            }

            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            return RedirectToAction("Index", "Admins");
        }
        [HttpPost]
        public IActionResult DeleteRecordLabel(int recordLabelId)
        {
            var model = new PagingModel<RecordLabel>() { Skip = MBoxConstants.initialSkip, Take = MBoxConstants.initialTakeTabel };
            model.PagingList = _recordLabelManager.GetSearchedRecordLabels(string.Empty, model.Skip, model.Take).ToList();
            var user = _userManager.FindByIdAsync(recordLabelId.ToString()).Result;
            
            if (user == null)
                return View("Index",model);

            _recordLabelManager.DeleteRecordLabel(user);
            return View("Index",model);

        }
        
    }
}