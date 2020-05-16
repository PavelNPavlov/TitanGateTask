using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.Data.Repository;
using WebsiteWebApI.Data.Repository.Contracts;

namespace WebsiteWebApI.Data
{
    public static class DataDIExtensions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }

    }
}
