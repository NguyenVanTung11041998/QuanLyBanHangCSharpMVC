using Models.DAO;
using System;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class StatisticalController : BaseController
    {
        private readonly StatisticalDAO statisticalDAO = new StatisticalDAO();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StatisticalBill(int? year)
        {
            var totalPrice = statisticalDAO.GetTotalPriceByYear(year ?? DateTime.Now.Year);
            return Json(totalPrice);
        }
    }
}