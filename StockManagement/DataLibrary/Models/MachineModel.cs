using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class MachineModel
    {
        public int Id_Machine { get; set; }
        public string Name_Machine { get; set; }
        public string Description_Machine { get; set; }
        public decimal TotalCost_Machine { get; set; }
        public int Id_Project { get; set; }
    }
}
