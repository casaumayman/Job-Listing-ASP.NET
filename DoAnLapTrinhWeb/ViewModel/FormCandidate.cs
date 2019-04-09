using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.ViewModel
{
    public class FormCandidate
    {
        [Key]
        public int Id_Candidate { get; set; }
        [Required, StringLength(50), Display(Name="Họ và tên")]
        public string Name { get; set; }
        [Phone, Required, Display(Name="Số điện thoại")]
        public string PhoneNumber { get; set; }
        public int YearBorn { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, StringLength(16), Display(Name ="Tên tài khoản")]
        public string UserName { get; set; }
        [Display(Name ="Giới tính")]
        public string Gender { get; set; }
        [Required, StringLength(16), Display(Name ="Mật khẩu")]
        public string Password { get; set; }
        [Required, StringLength(16), Display(Name = "Nhập lại mật khẩu")]
        public string RePassword { get; set; }
        public HttpPostedFileBase CV { get; set; }
        public string Id_School { get; set; }
        public string Id_Specialization { get; set; }

        public FormCandidate() { }
    }
}