using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Models
{
    public class SupplyModel
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

        [Display(Name = "Supply Total Price")]
        public decimal TotalPrice_Supply { get; set; }

        [Display(Name = "Material Total Price")]
        public decimal TotalPrice_Material { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Supply")]
        [Required(ErrorMessage = "You need to give the date of Supply.")]
        public DateTime Date_Supply { get; set; }

        [Display(Name = "Request number")]
        [Required(ErrorMessage = "You need to give the Request number.")]
        public int Request { get; set; }

        [Display(Name = "Order number")]
        [Required(ErrorMessage = "You need to give the Order number.")]
        public int SupplyOrder { get; set; }
    }
}