using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ProviderModel
    {
        [Display(Name = "Provider Name")]
        [Required(ErrorMessage = "You need to give the Provider Name.")]
        public string Name_Provider { get; set; }

        [Display(Name = "Provider Phone")]
        public string Phone_Provider { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Provider Mail")]
        public string Mail_Provider { get; set; }

        [Display(Name = "Provider Address")]
        public string Address_Provider { get; set; }

        [Display(Name = "Provider NIF")]
        public string Nif_Provider { get; set; }

        [Display(Name = "Provider Language")]
        public DataLibrary.Models.Language Language_Provider { get; set; }
    }
}