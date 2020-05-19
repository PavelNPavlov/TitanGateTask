using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebsiteWebApI.DataModels.Identity;

namespace WebsiteWebApI.BLServices.Contracts
{
    public interface IUserBLService
    {
        Task<IList<SelectListItem>> GetUserDropdown();

        Task<SystemUser> CreateUser(string email, string password);

        Task EditUser(Guid userId, string password, string oldPassword);

        Task<SystemUser> GetUserByUserName(string userName);
    }
}
