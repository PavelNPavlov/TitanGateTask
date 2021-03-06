﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.Infrastructure.OutputModels
{
    public class UrlOM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UrlPath { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
