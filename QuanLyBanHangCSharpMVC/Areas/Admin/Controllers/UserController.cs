using Models.DAO;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private AccountDAO accountDAO = new AccountDAO();
        [HttpGet]
        public ActionResult Index(string search, int currentPage = 1, int pageSize = 10)
        {
            var accounts = userDAO.GetAllUsersByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(accounts.TotalCount, pageSize);
            return View(accounts.Items);
        }

        [HttpPost]
        public async Task<ActionResult> GetUserInfo(long userId)
        {
            var user = await userDAO.GetUserInfoById(userId);
            return Json(new { user });
        }

        public ActionResult UserPartial(string search, int currentPage = 1, int pageSize = 10)
        {
            var accounts = userDAO.GetAllUsersByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(accounts.TotalCount, pageSize);
            return PartialView(accounts.Items);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeAccountStatus(string email)
        {
            bool result = await accountDAO.ChangeStatusAccount(email);
            return Json(result);
        }
    }
}