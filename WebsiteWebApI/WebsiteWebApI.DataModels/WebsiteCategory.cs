using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.DataModels
{
    public class WebsiteCategory : BaseDbModel
    {
        private ICollection<WebsiteEntity> websites;

        public WebsiteCategory()
        {
            this.websites = new HashSet<WebsiteEntity>();
        }

        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string Name { get; set; }

        public virtual ICollection<WebsiteEntity> Websites
        {
            get { return this.websites; }
            set { this.websites = value; }
        }
    }
}
