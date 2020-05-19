using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.BLServices.FileProviders;
using WebsiteWebApI.BLServices.Users;
using WebsiteWebApI.BLServices.Website;

namespace WebsiteWebApI.BLServices
{
    public static class BLServicesDIExtension
    {
        public static void AddBLServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryBLService, CategoryBLService>();
            services.AddScoped<IUserBLService, UsersBLService>();
            services.AddScoped<IWebsiteBlService, WebsiteBLService>();

            services.AddScoped<IFileBlService, FilesBLService>();
            services.AddScoped<SqlBlobFileProvider>();
        }
    }
}
