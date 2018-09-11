using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Application.Models.AdminViewModels;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminController : Controller
    {
        private IRecordLabelManager _recordLabelManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailsManager _emailManager;

        public AdminController(IRecordLabelManager recordLabelManager, UserManager<ApplicationUser> userManager, IEmailsManager emailManager)
        {
            _recordLabelManager = recordLabelManager;
            _userManager = userManager;
            _emailManager = emailManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            model.RecordLabels = _recordLabelManager.GetAllRecordLabels();

            return View(model);
        }
        [HttpGet]
        public IActionResult AddNewRecordLabel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewRecordLabelAsync(AddNewRecordLabelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Name = model.Name, UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, "PasswordNotSet!2#");
                if (result.Succeeded)
                {

                    var roleResult = _userManager.AddToRoleAsync(user, Role.RecordLabel.ToString()).Result;

                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                        return View(model);
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
                    await _emailManager.SendMail(EmailTemplateType.InvitedRecordLabel, model.Email, callbackUrl);
                }
                return View("SuccessfullyInvited");
            }
            return View(model);
        }

    }
}