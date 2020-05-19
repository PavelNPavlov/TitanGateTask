using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.Infrastructure.DataServiceModels;

namespace WebsiteWebApI.DataServices.Website.Contracts
{
    public interface ICategoryDataService
    {
        IList<CategoryDSModel> GetWebSiteCategories();
    }
}
