using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ViewStockMachineMaterialModel
    {
        [Display(Name = "Machine Name")]
        public string Name_Machine { get; set; }

        [Display(Name = "Material Name")]
        public string Name_Material { get; set; }

        [Display(Name = "Material Amount")]
        public int Amount_Material { get; set; }

        [Display(Name = "Material Total Cost")]
        public decimal Cost_Material { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Missing Material")]
        public int MissingMaterial { get; set; }

        [Display(Name = "Amount in Stock")]
        public int AmountinStock { get; set; }

        [RegularExpression("([1-9][0-9]*)")]
        public int StocktoMachine { get; set; }
        public bool Priority { get; set; }
        public int Request { get; set; }

        [Display(Name = "Name Provider")]
        public string Name_Provider { get; set; }
    }
}