using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelController : Controller
    {
        private IArtistManager _artistsManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordLabelController(IArtistManager artistsManager, UserManager<ApplicationUser> userManagerr)
        {      
            _artistsManager = artistsManager;
            _userManager = userManagerr;
        }

        public IActionResult Index()
        {
            var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));         

           DashboardViewModel model = new DashboardViewModel() { RecordLabelId = recordLabelId, Skip = 0, Take = 20 };
           model.Artists = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);

            return View(model);
        }

        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Artists";
            return View();
        }
        
    }
}