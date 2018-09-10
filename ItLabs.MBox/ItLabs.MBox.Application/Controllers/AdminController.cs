using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Application.Models.AdminViewModels;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminController : Controller
    {
        private IRecordLabelManager _recordLabelManager;

        public AdminController(IRecordLabelManager recordLabelManager)
        {
            _recordLabelManager = recordLabelManager;
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
        public IActionResult AddNewRecordLabel(AddNewRecordLabelViewModel model)
        {
            return View("SuccessfullyInvited");
        }

    }
}