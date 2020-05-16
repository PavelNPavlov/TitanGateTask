using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;

namespace WebsiteWebApI.DataModels
{
    public class FileBlob : BaseDbModel
    {
        public Guid FileId { get; set; }

        public byte[] Data { get; set; }
    }
}
