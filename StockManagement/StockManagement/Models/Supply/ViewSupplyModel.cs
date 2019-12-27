using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StockManagement.Models
{
    public class ViewSupplyModel
    {
        [Display(Name = "Provider")]
        public string Name_Provider { get; set; }

        [Display(Name = "Material")]
        public string Name_Material { get; set; }

        [Display(Name = "Specifications")]
        public string Specs_Material { get; set; }

        [Display(Name = "Section")]
        public string Name_Section { get; set; }

        [Display(Name = "Amount")]
        public int Amount_Material { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice_Supply { get; set; }

        [Display(Name = "Price Material")]
        public decimal TotalPrice_Material { get; set; }

        [Display(Name = "Date")]
        public DateTime Date_Supply { get; set; }

        [Display(Name = "Request number")]
        public int Request { get; set; }

        [Display(Name = "Check")]
        public bool Check { get; set; }

        [Display(Name = "Order number")]
        public int SupplyOrder { get; set; }

        [Display(Name = "Order number")]
        public string sortmethod { get; set; }

    }
}