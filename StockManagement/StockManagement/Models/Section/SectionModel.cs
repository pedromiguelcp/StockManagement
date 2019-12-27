using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class SectionModel
    {
        [Display(Name = "Section Name")]
        [Required(ErrorMessage = "You need to give the Section Name.")]
        public string Name_Section { get; set; }

        [Display(Name = "Section Description")]
        public string Description_Section { get; set; }
    }
}