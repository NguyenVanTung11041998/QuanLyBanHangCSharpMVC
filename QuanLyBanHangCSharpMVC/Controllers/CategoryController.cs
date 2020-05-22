using Models.DAO;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class CategoryController : Controller
    {
        private ProductDAO productDAO = new ProductDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id, int pageSize = 9, int currentPage = 1)
        {
            var products = await productDAO.GetAllProductByCategory(id, pageSize, currentPage);
            ViewBag.PageNumber = Paginate.GetTotalPage(products.TotalCount, pageSize);
            ViewBag.CategoryId = id;
            return View(products.Items);
        }

        public async Task<ActionResult> CategoryPartial(long id, int currentPage = 1, int pageSize = 9)
        {
            var products = await productDAO.GetAllProductByCategory(id, pageSize, currentPage);
            ViewBag.PageNumber = Paginate.GetTotalPage(products.TotalCount, pageSize);
            return PartialView(products.Items);
        }
    }
}