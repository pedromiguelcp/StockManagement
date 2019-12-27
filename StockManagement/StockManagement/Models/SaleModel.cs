using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class SaleModel
    {
        [Display(Name = "Provider Name")]
        [Required(ErrorMessage = "You need to give the Provider Name.")]
        public int Id_Provider { get; set; }

        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "You need to give the Material Name.")]
        public int Id_Material { get; set; }

        [Display(Name = "Section Name")]
        [Required(ErrorMessage = "You need to give the Section Name.")]
        public int Id_Section { get; set; }

        [Display(Name = "Amount of Material")]
        [Required(ErrorMessage = "You need to give the amount of material.")]
        public int Amount_Material { get; set; }

        [Display(Name = "Total Price of Material")]
        [Required(ErrorMessage = "You need to give the total price.")]
        public decimal TotalPrice_Supply { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Sale")]
        [Required(ErrorMessage = "You need to give the date of Sale.")]
        public DateTime Date_Supply { get; set; }
    }
}