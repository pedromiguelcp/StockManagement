using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class MachineMaterialModelPartial
    {
        [Display(Name = "Material")]
        [Required(ErrorMessage = "You need to give the Machine Name.")]
        public string Name_Material { get; set; }

        [Display(Name = "Specifications")]
        public string Specs_Material { get; set; }

        [Display(Name = "Check")]
        public bool Check { get; set; }

        [Display(Name = "Amount")]
        public int Amount_Material { get; set; }

        [Display(Name = "Total Cost")]
        public decimal Cost_Material { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal CostUnit_Material { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Section Name")]
        public int Id_Section { get; set; }

        [Display(Name = "Missing Material")]
        public int MissingMaterial { get; set; }

        [Display(Name = "Provider")]
        public int Id_Provider { get; set; }

        [Display(Name = "Request")]
        public int Request { get; set; }

        [Display(Name = "Priority")]
        public bool Priority { get; set; }

    }
}