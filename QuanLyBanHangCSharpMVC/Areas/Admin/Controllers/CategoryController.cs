using Models.Constants;
using Models.DAO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private CategoryDAO categoryDAO = new CategoryDAO();
        [HttpGet]
        public ActionResult Index(string search, int currentPage = 1, int pageSize = 20)
        {
            var categories = categoryDAO.GetAllCategoryByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(categories.TotalCount, pageSize);
            return View(categories.Items);
        }

        [HttpGet]
        public ActionResult CategoryPartial(string search, int currentPage = 1, int pageSize = 20)
        {
            var categories = categoryDAO.GetAllCategoryByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(categories.TotalCount, pageSize);
            return PartialView(categories.Items);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            category.CategoryStatus = CategoryStatus.Active;
            category.Id = await categoryDAO.CreateCategoryAsync(category);
            if (category.Id > 0)
            {
                TempData["SaveCategory"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var category = await categoryDAO.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["SaveCategory"] = "Không tồn tại danh mục!";
                return View("Index");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            bool result = await categoryDAO.EditCategoryAsync(category);
            if (result)
                TempData["SaveCategory"] = "Sửa danh mục thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            var result = await categoryDAO.DeleteAsync(id);
            if (result == ActionStatus.DeleteSuccess)
                TempData["DeleteCategory"] = "Xóa danh mục thành công!";
            else if (result == ActionStatus.ChangeStatus)
                TempData["DeleteCategory"] = "Đã khóa danh mục thành công!";
            else
                TempData["DeleteCategory"] = "Không tồn tại danh mục cần xóa!";
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(long id)
        {
            var result = await categoryDAO.ChangeStatusAsync(id);
            TempData["ChangeStatusCategory"] = "Cập nhật trạng thái thành công!";
            return Json(result);
        }
    }
}