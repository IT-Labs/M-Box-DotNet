using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Application.Models;

namespace ItLabs.MBox.Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Home";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About page";

            return View();
        }

        public IActionResult Artists()
        {
            ViewData["Message"] = "Artists";

            return View();
        }

        public IActionResult RecordLabels()
        {
            ViewData["Message"] = "RecordLabels";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
