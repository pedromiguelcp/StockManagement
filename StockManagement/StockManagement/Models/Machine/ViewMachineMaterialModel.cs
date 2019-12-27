using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ViewMachineMaterialModel
    {
        [Display(Name = "Machine Name")]
        public string Name_Machine { get; set; }

        [Display(Name = "Material Name")]
        public string Name_Material { get; set; }

        [Display(Name = "Amount")]
        public int Amount_Material { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal Unit_Cost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal Cost_Material { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Missing Material")]
        public int MissingMaterial { get; set; }

        [Display(Name = "Provider")]
        [Required]
        public int Id_Provider { get; set; }

        [Display(Name = "Request Number")]
        public int Request { get; set; }

        [Display(Name = "Priority")]
        public bool Priority { get; set; }
    }
}