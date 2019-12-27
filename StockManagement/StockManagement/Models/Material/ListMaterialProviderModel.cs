using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.Models.Material
{
    public class ListMaterialProviderModel
    {
        public MaterialModel MaterialModel { get; set; }

        public List<ViewMaterialModel> ListMaterialModel { get; set; }
    }
}