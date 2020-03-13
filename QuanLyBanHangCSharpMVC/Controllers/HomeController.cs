using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class HomeController : Controller
    {
        private CategoryDAO categoryDAO = new CategoryDAO();
        private ProductDAO productDAO = new ProductDAO();
        private ProducerDAO producerDAO = new ProducerDAO();
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult MenuPartial()
        {
            var category = categoryDAO.GetAllCategoryActive();
            return PartialView(category);
        }

        public ActionResult MenuLeftPartial()
        {
            var categories = categoryDAO.GetAllCategoryActive();
            var newProducts = productDAO.GetNewProducts();
            ViewBag.Products = newProducts;
            return PartialView(categories);
        }

        public ActionResult ProducerPartial()
        {
            var producer = producerDAO.GetAllProducerActive();
            return PartialView(producer);
        }

        public ActionResult NewProductPartial()
        {
            var products = productDAO.GetNewProducts(12);
            return PartialView(products);
        }
    }
}