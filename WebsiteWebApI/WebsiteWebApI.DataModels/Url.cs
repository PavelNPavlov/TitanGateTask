using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.Infrastructure;

namespace WebsiteWebApI.DataModels
{
    public class Url : BaseDbModel
    {
        [MaxLength(DataModelConstants.ShortStringLenght)]
        public string Name { get; set; }

        [MaxLength(DataModelConstants.LongStringLenght)]
        public string UrlPath { get; set; }
    }
}
