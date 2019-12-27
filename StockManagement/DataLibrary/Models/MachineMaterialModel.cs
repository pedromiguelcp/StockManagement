using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class MachineMaterialModel
    {
        public int Id_MachineMaterial { get; set; }
        public int Id_Machine { get; set; }
        public int Id_Material { get; set; }
        public int Id_Provider { get; set; }
        public int Amount_Material { get; set; }
        public decimal Cost_Material { get; set; }
        public string Observations { get; set; }
        public int MissingMaterial { get; set; }
        public bool Priority { get; set; }
        public int Request { get; set; }

    }
}
