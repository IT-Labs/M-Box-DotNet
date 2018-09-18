﻿using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ItLabs.MBox.Application.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        protected int CurrentLoggedUser => Convert.ToInt32(_userManager.GetUserId(HttpContext.User));
        

    }
}