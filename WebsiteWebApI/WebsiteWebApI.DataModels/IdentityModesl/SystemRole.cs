using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.DataModels.Identity
{
    public class SystemRole : IdentityRole<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}
