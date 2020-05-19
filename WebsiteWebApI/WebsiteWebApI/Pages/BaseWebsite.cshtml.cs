using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.Infrastructure.OutputModels;

namespace WebsiteWebApI.Pages
{
    public class BaseWebsiteModel : PageModel
    {
        private readonly IWebsiteBlService websiteBlService;

        public WebsiteListOM WebsiteData { get; set; }

        public BaseWebsiteModel(IWebsiteBlService websiteBlService)
        {
            this.websiteBlService = websiteBlService;
        }
        public void OnGet()
        {
            var url = (string)this.RouteData.Values["url"];

            this.WebsiteData = this.websiteBlService.GetWebsiteByUrl(url).Result;
        }
    }
}