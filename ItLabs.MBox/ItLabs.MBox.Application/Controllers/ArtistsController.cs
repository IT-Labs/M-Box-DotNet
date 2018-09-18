using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItLabs.MBox.Contracts.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.Artist))]
    public class ArtistsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult AddNewSong()
        {
            return View();
        }
    }
}