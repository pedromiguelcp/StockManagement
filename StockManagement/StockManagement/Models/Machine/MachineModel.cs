using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class MachineModel
    {
        [Display(Name = "Machine Name")]
        [Required(ErrorMessage = "You need to give the Machine Name.")]
        public string Name_Machine { get; set; }

        [Display(Name = "Machine Description")]
        public string Description_Machine { get; set; }

        [Display(Name = "Machine Total Cost")]
        public decimal TotalCost_Machine { get; set; }

        [Display(Name = "Associated Project Name")]
        [Required(ErrorMessage = "You need to give the Project Name.")]
        public int Id_Project { get; set; }
    }
}