using Model.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShop.Models
{
    public class UserProfile
    {

        [Display(Name = "Tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu cũ")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu cũ")]
        public string OldPassword { set; get; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu mới")]
        public string NewPassword { set; get; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("ConfirmPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        public string Name { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Điện thoại")]
        public string Phone { set; get; }
    }
}