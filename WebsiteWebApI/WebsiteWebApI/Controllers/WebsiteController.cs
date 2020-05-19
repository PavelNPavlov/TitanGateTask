﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.Infrastructure.InputModels;

namespace WebsiteWebApI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> logger;
        private readonly IWebsiteBlService websiteBlService;

        public WebsiteController(ILogger<WebsiteController> logger,
            IWebsiteBlService websiteBlService)
        {
            this.logger = logger;
            this.websiteBlService = websiteBlService;
        }


        [Route("{id}")]
        public async Task<IActionResult> GetWebsite(Guid id)
        {
            var result = await this.websiteBlService.GetSite(id);
            return new JsonResult(result);
        }

        [Route("{id}")]
        public async Task<IActionResult> DeleteWebsite(Guid id)
        {
            await this.websiteBlService.DeleteSite(id);
            return new JsonResult(id);
        }

        public async Task<IActionResult> GetWebsiteList([FromQuery]int page, [FromQuery]int pageSize)
        {
            var user = this.HttpContext.User.Identity.Name;
            try
            {
                var websites = await this.websiteBlService.GetSitesForOwner(user, page, pageSize);

                return new JsonResult(websites);
            }
            catch(Exception ex)
            {
                var s = 4;
            }
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateWebsiteIM createWebsiteIM)
        {
            try
            {
                await this.websiteBlService.CreateWebsite(createWebsiteIM);
            }
            catch(Exception ex)
            {
                var s = 5;
            }
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm]EditWebsiteIM input)
        {

            await this.websiteBlService.UpdateWebsite(input);
            return Ok();
        }
    }
}
