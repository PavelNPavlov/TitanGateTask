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
    public class WebsiteEditModel : PageModel
    {
        private readonly ILogger<WebsiteEditModel> logger;
        private readonly ICategoryBLService categoryBLService;
        private readonly IUserBLService userBLService;
        private readonly IWebsiteBlService websiteService;

        [BindProperty]
        public EditWebsiteIM CreateWebsiteIM { get; set; }

        [BindProperty]
        public IList<SelectListItem> Categories { get; set; }

        [BindProperty]
        public IList<SelectListItem> AvailableUsers { get; set; }

        public WebsiteEditModel(ICategoryBLService categoryBLService,
            IUserBLService userBLService,
            ILogger<WebsiteEditModel> logger,
            IWebsiteBlService websiteService)
        {
            this.logger = logger;
            this.categoryBLService = categoryBLService;
            this.userBLService = userBLService;
            this.websiteService = websiteService;

            this.CreateWebsiteIM = new EditWebsiteIM();
        }

        public void OnGet()
        {
            var id = new Guid((string)this.RouteData.Values["id"]);

            this.CreateWebsiteIM = this.websiteService.GetWebsiteDetails(id);
            var cat = this.categoryBLService.GetCategoryDropdown().ToList();
            var users = this.userBLService.GetUserDropdown().Result.ToList();

            cat.ForEach(x => x.Selected = new Guid(x.Value) == this.CreateWebsiteIM.CategoryId);
            users.ForEach(x => x.Selected = new Guid(x.Value) == this.CreateWebsiteIM.UserId);

            this.AvailableUsers = users;
            this.Categories = cat;
        }

        public void OnPost()
        {
            Page();
        }
    }
}