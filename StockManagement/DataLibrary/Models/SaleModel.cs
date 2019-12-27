using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    class SaleModel
    {
        public int Id_Sale { get; set; }
        public int Id_Client { get; set; }
        public int Id_Material { get; set; }
        public DateTime Date_Sale { get; set; }
    }
}
