using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItLabs.MBox.Application.Models.RecordLabelViewModels;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Domain.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.RecordLabel))]
    public class RecordLabelsController : Controller
    {
        private IArtistManager _artistsManager;
        private readonly MBoxUserManager _userManager;

        public RecordLabelsController(IArtistManager artistsManager, MBoxUserManager userManagerr)
        {      
            _artistsManager = artistsManager;
            _userManager = userManagerr;
        }

        public IActionResult Index()
        {
           var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));         

           DashboardViewModel model = new DashboardViewModel() { RecordLabelId = recordLabelId, Skip = 0, Take = 20 };
           model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take);

            return View(model);
        }

        [HttpGet]
        public IActionResult GetRecordLabelArtists([FromQuery] DashboardViewModel model)
        {
            var recordLabelId = System.Convert.ToInt32(_userManager.GetUserId(HttpContext.User));

            model.PagingList = _artistsManager.GetRecordLabelArtists(model.RecordLabelId, model.Skip, model.Take).ToList();

            return View("NextArtists", model);
        }



        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Artists";
            return View();
        }
        
    }
}