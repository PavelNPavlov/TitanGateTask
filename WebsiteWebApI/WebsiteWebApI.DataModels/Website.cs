using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.DataModels.Identity;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.DataModels
{
    public class Website : BaseDbModel
    {
        private ICollection<File> files;

        public Website()
        {
            this.files = new HashSet<File>();
        }

        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string Name { get; set; }

        public Guid WebsiteCategoryId { get; set; }

        public Guid UrlId { get; set; }

        public Guid SystemUserId { get; set; }

        public virtual SystemUser SystemUser { get; set; }

        public virtual ICollection<File> Files
        {
            get { return this.files; }
            set { this.files = value; }
        }

        public virtual Url Url { get; set; }

        public virtual WebsiteCategory WebsiteCategory { get; set; }
    }
}
