using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.Infrastructure.OutputModels
{
    public class WebsiteListOM
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string Owner { get; set; }
       
        public string Url { get; set; }

        public string SnapshotUrl { get; set; }
    }
}
