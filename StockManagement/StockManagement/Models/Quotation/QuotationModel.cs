using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class QuotationModel
    {
        public int Id_Quotation { get; set; }
        [Display(Name = "Provider Name")]
        public int Id_Provider { get; set; }
        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }
    }
}