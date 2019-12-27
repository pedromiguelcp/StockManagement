using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class QuotationModel
    {
        public int Id_Quotation { get; set; }
        public int Id_Provider { get; set; }
        public int Id_Material { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string QuotationPath { get; set; }
    }
}
