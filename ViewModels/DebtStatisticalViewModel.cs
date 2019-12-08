using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class DebtStatisticalViewModel
    {
        public int RentId { get; set; }
        public int No { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string Status { get; set; }

    }
}