using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Data
{
    public class AccountDto
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}