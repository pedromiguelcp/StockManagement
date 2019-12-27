using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public enum Language
    {
        PT = 1,
        EN = 2
    }
    public class ProviderModel
    {
        public int Id_Provider { get; set; }
        public string Name_Provider { get; set; }
        public string Phone_Provider { get; set; }
        public string Mail_Provider { get; set; }
        public string Address_Provider { get; set; }
        public string Nif_Provider { get; set; }
        public Language Language_Provider { get; set; }
    }
}
