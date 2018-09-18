﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Domain.Managers;
using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Controllers
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public class AdminsController : Controller
    {
        private IRecordLabelManager _recordLabelManager;
        private readonly MBoxUserManager _userManager;
        private readonly IEmailsManager _emailsManager;


        public AdminsController(IRecordLabelManager recordLabelManager, MBoxUserManager userManager, IEmailsManager emailManager)
        {
            _recordLabelManager = recordLabelManager;
            _userManager = userManager;
            _emailsManager = emailManager;
        }

        [HttpGet]
        public IActionResult Index()
        {   
            var model = new PagingModel<RecordLabel>() { Skip = 0, Take = 20 };
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetNextRecordLabels([FromQuery] PagingModel<RecordLabel> model)
        {
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take).ToList();
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
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
            await _emailsManager.PerpareSendMail(EmailTemplateType.InvitedRecordLabel, model.Email, callbackUrl);

            return View("SuccessfullyInvited");

        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            var model = new PagingModel<RecordLabel>() { Skip = 0, Take = 20 };

            if (search != null)
            {
                model.PagingList = _recordLabelManager.GetSearchedRecordLabels(search, model.Skip, model.Take);
                return View("Index", model);
            }

            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take);
            return RedirectToAction("Index", "Admins");
        }
        [HttpPost]
        public IActionResult DeleteRecordLabel(int recordLabelId)
        {
            var model = new PagingModel<RecordLabel>() { Skip = 0, Take = 20 };
            model.PagingList = _recordLabelManager.GetNextRecordLabels(model.Skip, model.Take);
            var user = _userManager.FindByIdAsync(recordLabelId.ToString()).Result;
            
            if (user == null)
                return View("Index",model);

            _recordLabelManager.DeleteRecordLabel(user);
            return View("Index",model);

        }
        
    }
}