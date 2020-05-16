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
        private ICollection<Website> websites;

        public WebsiteCategory()
        {
            this.websites = new HashSet<Website>();
        }

        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string Name { get; set; }

        public virtual ICollection<Website> Websites
        {
            get { return this.websites; }
            set { this.websites = value; }
        }
    }
}
