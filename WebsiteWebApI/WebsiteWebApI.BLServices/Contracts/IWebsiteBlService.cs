using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebsiteWebApI.Data.CrudAPIModels.Input;
using WebsiteWebApI.Infrastructure.InputModels;
using WebsiteWebApI.Infrastructure.OutputModels;

namespace WebsiteWebApI.BLServices.Contracts
{
    public interface IWebsiteBlService
    {
        Task CreateWebsite(CreateWebsiteIM data);

        EditWebsiteIM GetWebsiteDetails(Guid id);

        Task UpdateWebsite(EditWebsiteIM data);

        Task<IList<WebsiteListOM>> GetSitesForOwner(string userName, int page, int pageSize);

        Task<WebsiteListOM> GetSite(Guid id);

        Task DeleteSite(Guid id);

        Task<WebsiteListOM> GetWebsiteByUrl(string url);

        Task<IList<WebsiteListOM>> GetSites(GetInputModel data);
    }
}
