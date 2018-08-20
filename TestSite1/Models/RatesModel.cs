using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestSite1.Models
{

    public class BaseCurrency
    {
        [Key]
        public int id { get; set; }
        public string disclaimer { get; set; }
        public string license { get; set; }
        public int timestamp { get; set; }
        public string baseData { get; set; }
        public List<Rates> rates { get; set; }
    }

    public class Rates
    {
        [Key]
        public int id { get; set; }
        public double AED { get; set; }
        public double AFN { get; set; }
        public double ALL { get; set; }
        public double AMD { get; set; }
        public double ANG { get; set; }
        public double CAD { get; set; }
    }


}
