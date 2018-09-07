using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Application.Models.AdminViewModels;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminController : Controller
    {
        private IRecordLabelsManager _recordLabelManager;

        public AdminController(IRecordLabelsManager recordLabelManager)
        {
            _recordLabelManager = recordLabelManager;
        }

        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            
            model.GetNumberOfArtists = _recordLabelManager.RecordLabelNumberOfArtists();

            return View(model);
        }
    }
}