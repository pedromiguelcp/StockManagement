using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ProvideMaterialsModel
    {
        public int Id_ProvideMaterials { get; set; }
        public int Id_Provider { get; set; }
        public int Id_Material { get; set; }
        public decimal Unit_Cost { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string QuotationPath { get; set; }
    }
}
