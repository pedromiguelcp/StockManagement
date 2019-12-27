using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ProjectMachineMaterialModel
    {
        public MachineModel MachineModel { get; set; }

        public List<ViewMachineMaterialModel> ViewMachineMaterialModel { get; set; }
    }
}