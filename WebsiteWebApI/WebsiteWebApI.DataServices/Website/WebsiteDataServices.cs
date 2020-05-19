using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataServices.Website.Contracts;
using WebsiteWebApI.DataServices.Website.Models;
using WebsiteWebApI.DataModels;
using AutoMapper;

namespace WebsiteWebApI.DataServices.Website
{
    public class WebsiteDataServices : IWebsiteDataService
    {
        private readonly IGenericRepository<ApplicationDbContext, WebsiteEntity> websiteRepo;
        private readonly IGenericRepository<ApplicationDbContext, WebsiteCategory> categoryRepo;
        private readonly IMapper mapper;

        public WebsiteDataServices(IGenericRepository<ApplicationDbContext, WebsiteEntity> websiteRepo,
            IGenericRepository<ApplicationDbContext, WebsiteCategory> categoryRepo,
            IMapper mapper)
        {
            this.websiteRepo = websiteRepo;
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }

        public void CreateWebSite(CreateWebsiteModel data)
        {
            if(data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
        }
    }
}
