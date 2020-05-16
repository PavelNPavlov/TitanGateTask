using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.DataModels.Identity
{
    public class SystemUser : IdentityUser<Guid>
    {
        private ICollection<Website> websites;

        public SystemUser()
        {
            this.websites = new HashSet<Website>();
        }


        public virtual ICollection<Website> Websites
        {
            get { return this.websites; }
            set { this.websites = value; }
        }
    }
}
