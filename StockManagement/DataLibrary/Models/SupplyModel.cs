using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class SupplyModel
    {
        public int Id_Supply { get; set; }
        public int Id_Provider { get; set; }
        public int Id_Material { get; set; }
        public int Id_Section { get; set; }
        public int Amount_Material { get; set; }
        public decimal TotalPrice_Material { get; set; }
        public decimal TotalPrice_Supply { get; set; }
        public DateTime Date_Supply { get; set; }

        public int Request { get; set; }
        public int SupplyOrder { get; set; }
    }
}
