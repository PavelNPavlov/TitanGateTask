using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteWebApI.BLServices.Contracts
{
    public interface ICategoryBLService
    {
        IList<SelectListItem> GetCategoryDropdown();
    }
}
