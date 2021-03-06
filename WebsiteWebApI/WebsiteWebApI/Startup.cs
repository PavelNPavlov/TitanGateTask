using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WebsiteWebApI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebsiteWebApI.DataModels.Identity;
using WebsiteWebApI.DataServices;
using AutoMapper;
using WebsiteWebApI.AutoMapper;
using WebsiteWebApI.BLServices;
using WebsiteWebApI.Data.Seeder;
using Microsoft.AspNetCore.Mvc;

namespace WebsiteWebApI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    x=>x.MigrationsAssembly("WebsiteWebApI.Data")));

            services.AddIdentity<SystemUser, SystemRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            services.AddDataBaseServices();

            services.AddDataServices();

            services.AddBLServices();

            services.AddAutoMapper(typeof(ServerMapperProfile));

            services.AddControllers();

            services.AddRazorPages();
            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            UpdateDatabase(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    var userManger = serviceScope.ServiceProvider.GetService<UserManager<SystemUser>>();
                    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<SystemRole>>();
                    dbContext.Database.Migrate();
                    DataSeeder.Seed(dbContext, userManger, roleManager);
                }
            }
        }
    }
}
