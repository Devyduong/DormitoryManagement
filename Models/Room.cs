using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DormitoryManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [StringLength(100)]
        public string RoomName { get; set; }

        public int? Homefleet { get; set; }

        public int? ForGender { get; set; }

        public int? BedNumber { get; set; }

        public int? BedEmpty { get; set; }

        [StringLength(200)]
        public string Included { get; set; }

        public int? Status { get; set; }
    }
}