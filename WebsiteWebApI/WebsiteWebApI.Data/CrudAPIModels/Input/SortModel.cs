using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteWebApI.Data.CrudAPIModels.Input
{
    public class SortModel
    {
        public string Prop { get; set; }

        public int Orderd { get; set; }

        public string Direction { get; set; }
    }
}
