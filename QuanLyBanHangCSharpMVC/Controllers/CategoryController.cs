using Models.DAO;
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
            return View(products);
        }
    }
}