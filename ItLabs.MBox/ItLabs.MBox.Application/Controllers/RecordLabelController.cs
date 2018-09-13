using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelController : Controller
    {
        private IArtistManager _artistsManager;

        public RecordLabelController(IArtistManager artistsManager)
        {      
            _artistsManager = artistsManager;
        }

        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            model.Artists = _artistsManager.GetAllUserArtists();

            return View(model);
        }

        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Artists";
            return View();
        }
        
    }
}