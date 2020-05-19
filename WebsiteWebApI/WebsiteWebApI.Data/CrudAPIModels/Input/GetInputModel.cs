using System;
using System.Collections.Generic;

namespace WebsiteWebApI.Data.CrudAPIModels.Input
{
    public class GetInputModel
    {
        public Guid Id { get; set; }

        public IList<FilterModel> Filters { get; set; }

        public SortModel SortModel { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
