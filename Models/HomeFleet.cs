using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DormitoryManagement.Models
{
    public class HomeFleet
    {
        [Key]
        public int HFID { get; set; }

        [StringLength(50)]
        public string HFName { get; set; }

        public int? NumberOfRoom { get; set; }
        public int? PricePerRoom { get; set; }

        public int? Status { get; set; }
    }
}