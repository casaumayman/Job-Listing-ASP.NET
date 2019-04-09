using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.ViewModel
{
    public class FormEmployer
    { 
        [Required, StringLength(50), Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }
        [Required, StringLength(16), Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }
        [Required, StringLength(16), Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required, StringLength(16), Display(Name = "Nhập lại mật khẩu")]
        public string RePassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public HttpPostedFileBase ImgLogo { get; set; }

        public FormEmployer() { }
    }
}