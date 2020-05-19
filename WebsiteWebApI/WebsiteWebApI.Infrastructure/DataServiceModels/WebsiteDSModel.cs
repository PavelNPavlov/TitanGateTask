using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.Infrastructure.DataServiceModels
{
    public class WebsiteEditCreateDSModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid WebsiteCategoryId { get; set; }

        public Guid UrlId { get; set; }

        public string UrlText { get; set; }

        public Guid SystemUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
