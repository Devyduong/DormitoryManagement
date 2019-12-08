using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class PayViewModel
    {
        public int RentId { get; set; }
        [Display(Name = "Mã sinh viên")]
        public string StudentCode { get; set; }
        [Display(Name = "Tên sinh viên")]
        public string StudentName { get; set; }
        public string Status { get; set; }
        public double Debt { get; set; }
        public double? PayFee { get; set; }
    }
}