using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ProjectMachineModel
    {
        public ProjectModel ProjectModel { get; set; }
        public List<ViewMachineModel> ViewMachineModel { get; set; }
    }
}