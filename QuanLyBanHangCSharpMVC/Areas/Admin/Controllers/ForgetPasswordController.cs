using Models.DAO;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private AccountDAO accountDAO = new AccountDAO();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection f)
        {
            string email = f["txtEmail"];
            bool check = await accountDAO.CheckEmailExistAsync(email);
            if (!check)
            {
                ModelState.AddModelError("", "Email không tồn tại!");
                return View("Index");
            }
            string passWord = RandomPasswordHelper.CreateRandomPassword();
            string content = "Mật khẩu của bạn là: " + passWord;
            passWord = PasswordMd5.HashPassword(passWord);
            await accountDAO.UpdatePasswordByEmailAsync(email, passWord);
            try
            {
                await MailHelper.SendEmail(email, "Reset password", content);
                TempData["ResetPassword"] = "Yêu cầu thành công. Hệ thống sẽ gửi email cho bạn!";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                TempData["ResetPassword"] = "Đã có lỗi xảy ra, xin thử lại!";
                return View();
            }
        }
    }
}