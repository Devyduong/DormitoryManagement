using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DormitoryManagement.Models
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }
        public int? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(20)]
        public string CMND { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATECMND { get; set; }

        [StringLength(200)]
        public string IssuedBy { get; set; }

        [StringLength(100)]
        public string Religion { get; set; }

        [StringLength(100)]
        public string Folk { get; set; }

        [StringLength(100)]
        public string Nation { get; set; }

        public int? StudentYear { get; set; }

        [StringLength(200)]
        public string StudyAt { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Photo { get; set; }
        public int? Status { get; set; }

    }
}