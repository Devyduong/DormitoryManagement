using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class ListRentHomeViewModel
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Student { get; set; }

        public string Room { get; set; }
        public string HomeFleet { get; set; }

        [StringLength(50)]
        public string TheLease { get; set; }

        public string Paid { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int? Status { get; set; }
    }
}