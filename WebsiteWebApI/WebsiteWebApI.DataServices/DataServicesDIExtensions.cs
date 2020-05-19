using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WebsiteWebApI.DataServices.Website;
using WebsiteWebApI.DataServices.Website.Contracts;

namespace WebsiteWebApI.DataServices
{
    public static class DataServicesDIExtensions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IWebsiteDataService, WebsiteDataServices>();
            services.AddScoped<ICategoryDataService, CategoryDataService>();
        }
    }
}
