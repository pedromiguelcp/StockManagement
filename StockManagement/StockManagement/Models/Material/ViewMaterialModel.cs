using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ViewMaterialModel
    {
        public int Id_ProvideMaterial { get; set; }
        public int Id_Material { get; set; }

        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "You need to give the Material Name.")]
        public string Name_Material { get; set; }

        [Display(Name = "Material Specifications")]
        public string Specs_Material { get; set; }

        [Display(Name = "Section Name")]
        public string Name_Section { get; set; }

        [Display(Name = "Provider Name")]
        public string Name_Provider { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal Unit_Cost { get; set; }

        [Display(Name = "Quotation Path")]
        public string QuotationPath { get; set; }

        public HttpPostedFileBase QuotationFile { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }
    }
}