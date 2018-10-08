using ItLabs.MBox.Application.Models;
using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Domain.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Application.Controllers
{
    public class ListenersController : BaseController
    {   
        IS3Manager _s3Manager;
        public ListenersController(IS3Manager s3Manager,UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _s3Manager = s3Manager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index","HomeController");
        }
        [HttpGet]
        public IActionResult MyAccount()
        {
            var user = _userManager.Users.Where(x => x.Id == CurrentLoggedUserId).Include(x => x.Follows).FirstOrDefault();
            return View(user);
        }
        [HttpPost]
        public IActionResult ChangePicture(List<IFormFile> uploadedFiles)
        {
            var user = _userManager.FindByIdAsync(CurrentLoggedUserId.ToString()).Result;
            string imageS3Name = null;
            if (uploadedFiles.Count == 0)
            {
                ModelState.AddModelError("Picture", "Please choose a picture!");
                return View("MyAccount", user);
            }
            var formFile = uploadedFiles[0];
            if (!(formFile.ContentType.Equals("image/png") || formFile.ContentType.Equals("image/jpeg") || formFile.ContentType.Equals("image/jpeg")))
            {
                ModelState.AddModelError("Picture", formFile.ContentType + " extension is not allowed. You can only upload jpg, jpeg or png.");
                return View("MyAccount",user);
            }
            if (formFile.Length > MBoxConstants.MaximumImageSizeAllowed)
            {
                //Error Message
                ModelState.AddModelError("Picture", "Maximum 3MB picture size allowed!");
                return View("MyAccount", user);
            }

            imageS3Name = _s3Manager.UploadFile(formFile);

            user.Picture = imageS3Name;
            _userManager.UpdateAsync(user).Wait();
            return RedirectToAction("MyAccount");
        }

        [HttpPost]
        public IActionResult EditName(string listenerName)
        {
            var listener = _userManager.FindByIdAsync(CurrentLoggedUserId.ToString()).Result;

            if(string.IsNullOrWhiteSpace(listenerName) || listenerName.Length < 2)
            {
                ModelState.AddModelError("Name", "The Name must contain at least 2 characters");
                return View("MyAccount", listener);
            }
            listenerName = listenerName.Trim();
            if (listenerName.Length < 2)
            {
                ModelState.AddModelError("Name", "The Name must contain at least 2 characters");
                return View("MyAccount", listener);
            }
            if (listenerName.Length > 50)
            {
                ModelState.AddModelError("Name", "The Name cannot contain more than 50 characters");
                return View("MyAccount", listener);
            }

            listener.Name = listenerName;
            _userManager.UpdateAsync(listener).Wait();

            return RedirectToAction("MyAccount");
        }
        
    }
}