using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class MaterialModel
    {
        public int Id_Material { get; set; }
        public string Name_Material { get; set; }
        public string Specs_Material { get; set; }
        public int Id_Section { get; set; }
        public int Id_Provider { get; set; }
        public decimal Unit_Cost { get; set; }

    }
}
