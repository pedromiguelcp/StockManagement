using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class ProjectModel
    {
        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "You need to give the Project Name.")]
        public virtual string Name_Project { get; set; }

        [Display(Name = "Project Description")]
        public string Description_Project { get; set; }

        [Display(Name = "Project Total Cost")]
        public decimal TotalCost_Project { get; set; }
    }
}