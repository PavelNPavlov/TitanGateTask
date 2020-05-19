using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.Infrastructure.InputModels;
using WebsiteWebApI.Infrastructure.OutputModels;

namespace WebsiteWebApI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebsiteBlService websiteBlService;

        public IList<WebsiteListOM> Websites { get; set; }

        public IndexModel(ILogger<IndexModel> logger,
            IWebsiteBlService websiteBlService)
        {
            _logger = logger;
            this.websiteBlService = websiteBlService;

            this.Websites = new List<WebsiteListOM>();
        }

        public void OnGet()
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userName = this.HttpContext.User.Identity.Name;

            var page = 1;
            var pageSize = 10;

            if (this.Request.Query.ContainsKey("page"))
            {
                page = int.Parse(this.Request.Query["page"]);
            }

            if (this.Request.Query.ContainsKey("pageSize"))
            {
                pageSize = int.Parse(this.Request.Query["pageSize"]);
            }

            this.Websites = this.websiteBlService.GetSitesForOwner(userName, page, pageSize).Result;
        }
    }
}
