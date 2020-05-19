using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.DataServices.Website.Models
{
    public class CreateWebsiteModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Guid UserId { get; set; }

        public Guid? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
