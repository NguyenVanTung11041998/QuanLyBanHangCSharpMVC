using Models.DAO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Areas.Admin.Data;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        private UserDAO userDAO = new UserDAO();
        private AccountDAO accountDAO = new AccountDAO();
        [HttpGet]
        public ActionResult Index()
        {
            Account account = (Account)Session[Constant.UserAdminSession];
            UserDto userDto = new UserDto
            {
                Id = account?.UserId ?? 0,
                Address = account?.User.Address,
                Email = account?.Email,
                Phone = account?.User.Phone,
                Name = account?.User.Name
            };
            return View(userDto);
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserDto userDto)
        {
            bool checkEmail = await accountDAO.CheckEmailExistAsync(userDto.Email);
            if (!checkEmail)
            {
                if (ModelState.IsValid)
                {
                    bool checkPhone = !string.IsNullOrWhiteSpace(userDto.Phone) ? await accountDAO.CheckPhoneExistAsync(userDto.Phone.Trim()) : true;
                    if (checkPhone && string.IsNullOrWhiteSpace(userDto.Phone) || !checkPhone)
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
                            AccountRole = AccountRole.Admin,
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
                        ModelState.AddModelError("AddUserFail", "Số điện thoại đã tồn tại!");
                }
            }
            else
                ModelState.AddModelError("EmailExits", "Email đã tồn tại!");
            return View(userDto);
        }
    }
}