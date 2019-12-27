using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class IntegratorModel
    {
        [Display(Name = "Integrator Name")]
        [Required(ErrorMessage = "You need to give the Integrator Name.")]
        public string Name_Integrator { get; set; }

        [Display(Name = "Integrator Phone")]
        public string Phone_Integrator { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Integrator Mail")]
        public string Mail_Integrator { get; set; }

        [Display(Name = "Integrator Address")]
        public string Address_Integrator { get; set; }

        [Display(Name = "Integrator NIF")]
        public string Nif_Integrator { get; set; }
    }
}