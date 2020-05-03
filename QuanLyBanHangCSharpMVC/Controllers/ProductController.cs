using Models.DAO;
using QuanLyBanHangCSharpMVC.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDAO productDAO = new ProductDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            var product = await productDAO.GetProductByIdAndRelated(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult GetProductByKeyword(string search)
        {
            var products = productDAO.GetAllProductImageByKeyWord(search);
            return View(products);
        }
    }
}