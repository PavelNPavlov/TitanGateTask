using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Contracts;
using WebsiteWebApI.Data.CrudAPIModels.Input;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.Infrastructure.DataServiceModels;
using WebsiteWebApI.Infrastructure.InputModels;
using WebsiteWebApI.Infrastructure.OutputModels;

namespace WebsiteWebApI.BLServices.Website
{
    public class WebsiteBLService : IWebsiteBlService
    {
        private readonly IUserBLService userBLService;
        private readonly IFileBlService fileBlService;
        private readonly IMapper mapper;
        private readonly IBaseDataService<UrlCreateEditDSModel, UrlCreateEditDSModel, UrlOM, Url, ApplicationDbContext> urlDataService;

        private readonly IBaseDataService<WebsiteEditCreateDSModel, WebsiteEditCreateDSModel, WebsiteEntity, WebsiteEntity, ApplicationDbContext> websiteDataService;
        private readonly IBaseDataService<WebsiteEditCreateDSModel, WebsiteEditCreateDSModel, WebsiteListOM, WebsiteEntity, ApplicationDbContext> websiteListDataService;

        public WebsiteBLService(IUserBLService userBLService,
            IBaseDataService<UrlCreateEditDSModel, UrlCreateEditDSModel, UrlOM, Url, ApplicationDbContext> urlDataService,
            IBaseDataService<WebsiteEditCreateDSModel, WebsiteEditCreateDSModel, WebsiteEntity, WebsiteEntity, ApplicationDbContext> websiteDataService,
            IBaseDataService<WebsiteEditCreateDSModel, WebsiteEditCreateDSModel, WebsiteListOM, WebsiteEntity, ApplicationDbContext> websiteListDataService,
            IMapper mapper,
            IFileBlService fileBlService)
        {
            this.userBLService = userBLService;
            this.mapper = mapper;
            this.urlDataService = urlDataService;
            this.websiteDataService = websiteDataService;
            this.fileBlService = fileBlService;
            this.websiteListDataService = websiteListDataService;
        }

        public async Task<IList<WebsiteListOM>> GetSitesForOwner(string userName, int page, int pageSize)
        {
            var user = await this.userBLService.GetUserByUserName(userName);


            var filters = new List<FilterModel>();

            var userIdFilter = new FilterModel()
            {
                Comparison = "equals",
                Prop = "SystemUserId",
                Value = user.Id
            };

            filters.Add(userIdFilter);

            var result = this.websiteListDataService.GetItems(filters, null, page, pageSize);

            return result;
        }

        public async Task<WebsiteListOM> GetSite(Guid id)       
        {
            var result = this.websiteListDataService.GetItemById(new GetInputModel { Id = id});

            return result;
        }

        public async Task<IList<WebsiteListOM>> GetSites(GetInputModel data)
        {
            var result = this.websiteListDataService.GetItems(data.Filters, data.SortModel, data.Page, data.PageSize);

            return result;
        }

        public async Task DeleteSite(Guid id)
        {
            var website = this.websiteDataService.GetItemById(new GetInputModel { Id = id });

            this.urlDataService.DeleteItem(new DeleteInputModel() { Id = website.UrlId });
            
            foreach (var item in website.Files)
            {
                this.fileBlService.DeleteFile(item.Id);
            }

            this.websiteDataService.DeleteItem(new DeleteInputModel { Id = id });
        }


        public async Task CreateWebsite(CreateWebsiteIM data)
        {
            if (data.UserId == Guid.Empty)
            {
                var user = await this.userBLService.CreateUser(data.UserName, data.Password);
                data.UserId = user.Id;
            }

            var webSite = new WebsiteEditCreateDSModel()
            {
                Name = data.Name,
                Id = Guid.NewGuid(),
            };

            var urlData = this.mapper.Map<UrlCreateEditDSModel>(data);

            this.urlDataService.CreateItemWithNoReturn(urlData);

            webSite.UrlId = urlData.Id;
            webSite.SystemUserId = data.UserId;
            webSite.WebsiteCategoryId = data.CategoryId;

            this.websiteDataService.CreateItemWithNoReturn(webSite);

            var file = this.fileBlService.UploadFile(data.HomePageSnapshot, "HomePageSnapShot", webSite.Id, data.FileProviderName);

            this.websiteDataService.SaveChanges();

        }

        public EditWebsiteIM GetWebsiteDetails(Guid id)
        {
            var website = this.websiteDataService.GetItemById(new GetInputModel { Id = id });

            var result = this.mapper.Map<EditWebsiteIM>(website);

            return result;
        }

        public async Task UpdateWebsite(EditWebsiteIM data)
        {
            var website = this.websiteDataService.GetItemById(new GetInputModel { Id = data.Id });

            if (!string.IsNullOrEmpty(data.CurrentPassword) && !string.IsNullOrEmpty(data.NewPassword))
            {
                await this.userBLService.EditUser(data.UserId, data.NewPassword, data.CurrentPassword);
            }

            if (data.HomePageSnapshot != null)
            {
                var file = this.fileBlService.UploadFile(data.HomePageSnapshot, "HomePageSnapShot", website.Id, data.FileProviderName);

            }
            website.Name = data.Name;

            website.Url.UrlPath = data.URL;
            website.SystemUserId = data.UserId;
            website.WebsiteCategoryId = data.CategoryId;

            this.websiteDataService.EditItemNoReturn(website);

            return;
        }

        public async Task<WebsiteListOM> GetWebsiteByUrl(string url)
        {
            var filter = new List<FilterModel>()
            {
                new FilterModel(){Prop ="Url.UrlPath", Comparison = "equals", Value = url}
            };

            var website = this.websiteListDataService.GetItems(filter, null,1,1);

            if(website.Count == 0)
            {
                return null;
            }

            return website.First();           
        }



    }
}
