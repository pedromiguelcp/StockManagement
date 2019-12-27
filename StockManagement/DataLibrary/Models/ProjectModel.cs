using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ProjectModel
    {
        public int Id_Project { get; set; }
        public string Name_Project { get; set; }
        public string Description_Project { get; set; }
        public decimal TotalCost_Project { get; set; }
    }
}
