using Models.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class ProductController : Controller
    {
        private ProductDAO productDAO = new ProductDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            var product = await productDAO.GetProductByIdAndRelated(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(product);
        }
    }
}