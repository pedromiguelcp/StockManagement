using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ProjectStockMachineMaterialModel
    {
        public MachineModel MachineModel { get; set; }

        public List<ViewStockMachineMaterialModel> ViewStockMachineMaterialModel { get; set; }



    }
}