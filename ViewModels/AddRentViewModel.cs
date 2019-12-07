using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class AddRentViewModel
    {
        public string Student { get; set; }
        public int Homefleet { get; set; }
        public int Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Paid { get; set; }

    }
}