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

namespace WebsiteWebApI.Pages
{
    public class WebsiteCreateModel : PageModel
    {
        private readonly ILogger<WebsiteCreateModel> logger;
        private readonly ICategoryBLService categoryBLService;
        private readonly IUserBLService userBLService;

        [BindProperty]
        public CreateWebsiteIM CreateWebsiteIM { get; set; }

        [BindProperty]
        public IList<SelectListItem> Categories { get; set; }

        [BindProperty]
        public IList<SelectListItem> AvailableUsers { get; set; }

        public WebsiteCreateModel(ICategoryBLService categoryBLService,
            IUserBLService userBLService,
            ILogger<WebsiteCreateModel> logger)
        {
            this.logger = logger;
            this.categoryBLService = categoryBLService;
            this.userBLService = userBLService;

            this.CreateWebsiteIM = new CreateWebsiteIM();
        }

        public void OnGet()
        {
            var cat = this.categoryBLService.GetCategoryDropdown();
            var users = this.userBLService.GetUserDropdown().Result;

            this.AvailableUsers = users;
            this.Categories = cat;
        }

        public void OnPost()
        {
            Page();
        }
    }
}