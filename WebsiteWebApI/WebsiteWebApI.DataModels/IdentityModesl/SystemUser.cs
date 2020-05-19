using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.DataModels.Identity
{
    public class SystemUser : IdentityUser<Guid>
    {
        private ICollection<WebsiteEntity> websites;

        public SystemUser()
        {
            this.websites = new HashSet<WebsiteEntity>();
        }

        public bool IsDeleted { get; set; }
        public virtual ICollection<WebsiteEntity> Websites
        {
            get { return this.websites; }
            set { this.websites = value; }
        }
    }
}
