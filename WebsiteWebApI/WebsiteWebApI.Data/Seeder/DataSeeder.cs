using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.DataModels.Identity;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.Data.Seeder
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context, UserManager<SystemUser> userManager, RoleManager<SystemRole> roleManager)
        {
            SeedCategories(context);
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedFileProvider(context);
        }

        private static void SeedRoles(RoleManager<SystemRole> roleManager)
        {
            if(roleManager.Roles.Count() > 0)
            {
                return;
            }

            var adminRole = new SystemRole
            {
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var siteAdminRole = new SystemRole
            {
                Name = "SiteAdmin",
                NormalizedName = "SiteAdmin"
            };

            var r1 = roleManager.CreateAsync(adminRole).Result;
            var r2 = roleManager.CreateAsync(siteAdminRole).Result;


        }
        private static void SeedUsers(UserManager<SystemUser> userManager)
        {
            if (userManager.Users.Count() > 0)
            {
                return;
            }

            var user1 = new SystemUser
            {
                Email = "test@test.com",
                UserName = "test@test.com"
            };            

            var user = userManager.CreateAsync(user1, "Qwerty!123").Result;
            var role = userManager.AddToRoleAsync(user1, "Admin").Result;
            var role2 = userManager.AddToRoleAsync(user1, "SiteAdmin").Result;
        }
        private static void SeedCategories(ApplicationDbContext context)
        {
            if(context.WebsiteCategories.Count() > 0)
            {
                return;
            }

            var testCategory1 = new WebsiteCategory
            {
                Name = "Sports",
                DateCreated = UtilityConstants.Now,
                DateModified = UtilityConstants.Now
            };

            var testCategory2 = new WebsiteCategory
            {
                Name = "Leasure",
                DateCreated = UtilityConstants.Now,
                DateModified = UtilityConstants.Now
            };

            context.WebsiteCategories.Add(testCategory1);
            context.WebsiteCategories.Add(testCategory2);

            context.SaveChanges();
        }

        private static void SeedFileProvider(ApplicationDbContext context)
        {
            if (context.FileProviders.Count() > 0)
            {
                return;
            }

            var provider1 = new FileProvider
            {
                ImplementationServiceName = "WebsiteWebApI.BLServices.FileProviders.SqlBlobFileProvider",
                Name = "SqlBlobFileProvider",
                DateCreated = UtilityConstants.Now,
                DateModified = UtilityConstants.Now
            };

            context.FileProviders.Add(provider1);

            context.SaveChanges();
        }
    }
}
