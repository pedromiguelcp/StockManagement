using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class SupplyMaterialModel
    {
        public SupplyModel SupplyModel { get; set; }

        public List<ViewSupplyModel> ViewSupplyModel { get; set; }
    }
}