﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CodeSanook.AmazonS3.Services;
using CodeSanook.AmazonS3.ViewModels;
using Orchard.UI.Admin;

namespace CodeSanook.AmazonS3.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        private readonly IAmazonS3StorageConfiguration _settingsService;

        public AdminController(IAmazonS3StorageConfiguration settingsService)
        {
            _settingsService = settingsService;
        }

        public ActionResult Settings()
        {
            var viewModel = new SettingsViewModel(_settingsService);
            return View(viewModel);
        }       

        [HttpPost]
        public ActionResult Settings(SettingsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            _settingsService.AWSAccessKey = viewModel.AWSAccessKey;
            _settingsService.AWSFileBucket = viewModel.AWSFileBucket;
            _settingsService.AWSS3PublicUrl = viewModel.AWSS3PublicUrl;
            _settingsService.AWSSecretKey = viewModel.AWSSecretKey;
            _settingsService.Save();
            return RedirectToAction("Settings");
        }
    }

}
