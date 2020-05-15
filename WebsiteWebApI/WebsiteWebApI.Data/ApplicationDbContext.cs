using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteWebApI.DataModels.Identity;

namespace WebsiteWebApI.Data
{
    public class ApplicationDbContext : IdentityDbContext<SystemUser,SystemRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
