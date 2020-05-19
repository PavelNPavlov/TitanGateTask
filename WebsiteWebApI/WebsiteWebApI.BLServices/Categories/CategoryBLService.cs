using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.DataServices.Website.Contracts;

namespace WebsiteWebApI.BLServices
{
    public class CategoryBLService : ICategoryBLService
    {
        private readonly ICategoryDataService categoryDataService;
        private readonly IMapper mapper;

        public CategoryBLService(ICategoryDataService categoryDataService,
            IMapper mapper)
        {
            this.categoryDataService = categoryDataService;
            this.mapper = mapper;
        }

        public IList<SelectListItem> GetCategoryDropdown()
        {
            var data = this.categoryDataService.GetWebSiteCategories();

            var result = this.mapper.Map<List<SelectListItem>>(data);

            return result;
        }
    }
}
