using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.DataServices.Website.Contracts;
using WebsiteWebApI.Infrastructure.DataServiceModels;

namespace WebsiteWebApI.DataServices.Website
{
    public class CategoryDataService : ICategoryDataService
    {
        private readonly IGenericRepository<ApplicationDbContext, WebsiteCategory> websiteCategory;
        private readonly IMapper mapper;


        public CategoryDataService(IGenericRepository<ApplicationDbContext, WebsiteCategory> websiteCategory,
            IMapper mapper)
        {
            this.websiteCategory = websiteCategory;
            this.mapper = mapper;
        }

        public IList<CategoryDSModel> GetWebSiteCategories()
        {
            var categories = this.websiteCategory.All();

            var result = this.mapper.ProjectTo<CategoryDSModel>(categories).ToList();
            return result;
        }
    }
}
