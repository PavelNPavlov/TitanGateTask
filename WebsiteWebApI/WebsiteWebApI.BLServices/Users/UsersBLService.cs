using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.DataModels.Identity;

namespace WebsiteWebApI.BLServices.Users
{
    public class UsersBLService : IUserBLService
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly IMapper mapper;

        public UsersBLService(UserManager<SystemUser> userManager,
            RoleManager<SystemRole> roleManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<IList<SelectListItem>> GetUserDropdown()
        {
            var siteAdminUsers = await this.userManager.GetUsersInRoleAsync("SiteAdmin");

            var items = this.mapper.Map<IList<SelectListItem>>(siteAdminUsers);

            items.Add(new SelectListItem
            {
                Value = Guid.Empty.ToString(),
                Text = string.Empty
            });

            var result = items.OrderBy(x => x.Text).ToList();

            return result;
        }

        public async Task<SystemUser> CreateUser(string email, string password)
        {
            var newUser = new SystemUser
            {
                UserName = email,
                Email = email
            };

            var userResult = await this.userManager.CreateAsync(newUser, password);
            await this.userManager.AddToRoleAsync(newUser, "SiteAdmin");

            return newUser;
        }

        public async Task EditUser(Guid userId, string password, string oldPassword)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == userId);

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, oldPassword, password);
        }

        public async Task<SystemUser> GetUserByUserName(string userName)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.UserName == userName);

            return user;
        }
    }
}
