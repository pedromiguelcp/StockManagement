using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Models
{
    public class ViewStockMaterialModel
    {

        [Display(Name = "Provider Name")]
        public string Name_Provider { get; set; }

        [Display(Name = "Material Name")]
        public string Name_Material { get; set; }

        [Display(Name = "Section Name")]
        public string Name_Section { get; set; }

        [Display(Name = "Section Name")]
        public int Id_Section { get; set; }

        [Display(Name = "Amount of Material")]
        public int Amount_Material { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

    }
}