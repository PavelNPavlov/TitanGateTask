using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.DataModels
{
    public class File : BaseDbModel
    {
        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string Name { get; set; }

        [MaxLength(DataModelConstants.MediumStringLenght)]
        public string FileName { get; set; }

        [MaxLength(DataModelConstants.ShortStringLenght)]
        public string FileExtension { get; set; }

        [MaxLength(DataModelConstants.ShortStringLenght)]
        public string FileType { get; set; }

        public Guid FileProviderId { get; set; }

        public Guid WebsiteId { get; set; }

        public virtual Website Website { get; set; }

        public virtual FileProvider FileProvider { get; set; }
    }
}
