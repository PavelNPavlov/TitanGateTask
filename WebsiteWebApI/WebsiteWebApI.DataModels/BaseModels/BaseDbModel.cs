using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.DataModels.BaseModels
{
    public abstract class BaseDbModel
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}
