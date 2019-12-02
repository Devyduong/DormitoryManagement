using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DormitoryManagement.ViewModels
{
    public class EditRoomViewModel
    {
        public int RoomId { get; set; }
        [Display(Name = "Tên phòng")]
        [Required(ErrorMessage = "Vui lòng nhập tên phòng")]
        [MaxLength(100, ErrorMessage = "Tên phòng quá dài")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tòa nhà")]
        [Display(Name = "Thuộc tòa nhà")]
        public int? Homefleet { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [Display(Name = "Phòng(Nam/Nữ)")]
        public string ForGender { get; set; }

        [Display(Name = "Số giường")]
        [Required(ErrorMessage = "Vui lòng nhập số giường")]
        [Range(1, 100, ErrorMessage = "Ít nhất phải có 1 giường trong phòng")]
        public int? BedNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Cơ sở vật chất có sẵn")]
        public string Included { get; set; }
    }
}