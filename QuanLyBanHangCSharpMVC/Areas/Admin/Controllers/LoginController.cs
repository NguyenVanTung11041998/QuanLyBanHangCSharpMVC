using Models.DAO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Areas.Admin.Data;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private AccountDAO accountDAO = new AccountDAO();
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() =>
            {
                AccountDto account = new AccountDto();
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    account.Email = ticket.Name;
                    account.Password = ticket.UserData;
                    account.RememberMe = true;
                }
                return View(account);
            });
        }

        [HttpPost]
        public async Task<ActionResult> Index(AccountDto account)
        {
            if (ModelState.IsValid)
            {
                string password = PasswordMd5.HashPassword(account.Password);
                Account accountLogin = await accountDAO.LoginAsync(account.Email, password);
                if (accountLogin != null && (accountLogin.AccountRole == AccountRole.Manager || accountLogin.AccountRole == AccountRole.Admin))
                {
                    Session.Add(Constant.UserAdminSession, accountLogin);
                    var authTicket = new FormsAuthenticationTicket(1, $"{accountLogin.Email}", DateTime.Now, DateTime.Now.AddMinutes(20), account.RememberMe, account.Password, "/");
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
            }
            return View(account);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(Constant.UserAdminSession);
            return RedirectToAction("Index", "Login");
        }
    }
}