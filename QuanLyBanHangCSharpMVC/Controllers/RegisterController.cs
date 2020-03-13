using Models.DAO;
using Models.DTO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Areas.Admin.Data;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class RegisterController : Controller
    {
        private AccountDAO accountDAO = new AccountDAO();
        private UserDAO userDAO = new UserDAO();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserDto userDto)
        {
            bool checkEmail = await accountDAO.CheckEmailExistAsync(userDto.Email);
            if (!checkEmail)
            {
                User user = new User
                {
                    Address = userDto.Address,
                    Name = userDto.Name,
                    Phone = userDto.Phone,
                    UserSatus = UserSatus.Active
                };
                long userId = await userDAO.CreateUserAsync(user);
                userDto.Password = PasswordMd5.HashPassword(userDto.Password);
                Account account = new Account
                {
                    AccountRole = AccountRole.User,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    UserId = userId,
                    AccountStatus = AccountStatus.Active
                };
                bool result = await accountDAO.CreateAccountAsync(account);
                if (result)
                {
                    TempData["AddAcountSuccess"] = "Đăng ký tài khoản thành công!";
                    return RedirectToAction("Index", "Login");
                }
                else
                    ModelState.AddModelError("AddUserFail", "Đăng ký không thành công!");
            }
            else
                ModelState.AddModelError("EmailExits", "Email đã tồn tại!");
            return View(userDto);
        }

        [HttpGet]
        public ActionResult Edit()
        {

            var account = (Account)Session[Constant.UserCustomerSession];
            if (account == null)
                return RedirectToAction("Index", "Login");
            AccountUserDto user = new AccountUserDto
            {
                Address = account.User.Address,
                Email = account.Email,
                Name = account.User.Name,
                Phone = account.User.Phone,
                Id = account.UserId
            };
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AccountUserDto userDto)
        {
            if(!string.IsNullOrWhiteSpace(userDto.NewPassword))
                userDto.Password = PasswordMd5.HashPassword(userDto.NewPassword);
            bool result = await userDAO.EditAsync(userDto);
            if (result)
                TempData["EditAccount"] = "Cập nhật thông tin thành công!";
            else
                TempData["EditAccount"] = "Đã hủy cập nhật thông tin!";
            return RedirectToAction("Index", "Home");
        }
    }
}