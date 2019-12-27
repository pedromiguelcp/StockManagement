using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class StockMaterialModel
    {
        public int Id_StockMaterial { get; set; }
        public int Id_Material { get; set; }
        public int Id_Provider { get; set; }
        public int Id_Section { get; set; }
        public int Amount_Material { get; set; }
        public string Observations { get; set; }
    }
}
