using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class MachineMaterialModel
    {
        
        [Display(Name = "Machine Name")]
        [Required(ErrorMessage = "You need to give the Machine Name.")]
        public int Id_Machine { get; set; }

        [Display(Name = "Section Name")]
        [Required(ErrorMessage = "You need to give the Section Name.")]
        public int Id_Section { get; set; }

        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "You need to give the Material Name.")]
        public int Id_Material { get; set; }

        [Display(Name = "Material Amount")]
        public int Amount_Material { get; set; }

        [Display(Name = "Material Unit Cost")]
        public decimal UnitCost_Material { get; set; }

        [Display(Name = "Material Total Cost")]
        public decimal Cost_Material { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }
        [Display(Name = "Missing Material")]
        public int MissingMaterial { get; set; }

        [Display(Name = "Name Provider")]
        public int Id_Provider { get; set; }

        [Display(Name = "Request")]
        public int Request { get; set; }

        [Display(Name = "Priority")]
        public bool Priority { get; set; }
    }
}