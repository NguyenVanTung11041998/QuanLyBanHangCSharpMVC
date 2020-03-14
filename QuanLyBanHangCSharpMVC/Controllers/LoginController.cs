using Models.DAO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Helpers;
using QuanLyBanHangCSharpMVC.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class LoginController : Controller
    {
        private AccountDAO accountDAO = new AccountDAO();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(AccountDto account)
        {
            string password = PasswordMd5.HashPassword(account.Password);
            Account accountLogin = await accountDAO.LoginAsync(account.Email, password, false);
            if (accountLogin != null)
            {
                Session.Add(Constant.UserCustomerSession, accountLogin);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(Constant.UserCustomerSession);
            return RedirectToAction("Index", "Login");
        }
    }
}