using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class ListStudentViewModel
    {
        public string StudentId { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }
        public int? Gender { get; set; }

        public string DOB { get; set; }

        [StringLength(20)]
        public string CMND { get; set; }

        [StringLength(100)]
        public string Religion { get; set; }
        public int? StudentYear { get; set; }

        [StringLength(200)]
        public string StudyAt { get; set; }

        [StringLength(50)]
        public string Department { get; set; }
        public string Email { get; set; }
    }
}