using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Data
{
    public class UserDto
    {
        public long Id { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên không được để trống!")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}