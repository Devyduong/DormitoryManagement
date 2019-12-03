using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DormitoryManagement.Models
{
    public class Rent
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Renter { get; set; }

        public int? Room { get; set; }

        [StringLength(50)]
        public string TheLease { get; set; }

        public double? TotalFee { get; set; }

        public double? Paid { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int? Status { get; set; }
    }
}