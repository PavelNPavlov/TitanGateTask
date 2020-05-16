using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.DataModels
{
    public class FileProvider : BaseDbModel
    {
        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string Name { get; set; }

        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string ImplementationServiceName { get; set; }
    }
}
