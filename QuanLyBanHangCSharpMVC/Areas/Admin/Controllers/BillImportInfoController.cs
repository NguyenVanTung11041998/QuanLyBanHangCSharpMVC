using Models.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class BillImportInfoController : BaseController
    {
        private BillImportDAO billImportDAO = new BillImportDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            var bill = await billImportDAO.GetAllBillImportInfoByBillImportId(id);
            if (bill == null)
                return RedirectToAction("Index", "BillImport");
            ViewBag.BillImport = bill;
            return View(bill.BillImportInfos);
        }

        [HttpPost]
        public ActionResult AcceptBillImport(long id)
        {
            bool result = billImportDAO.AcceptBillImport(id);
            TempData["BillImport"] = "Đã xác nhận phiếu nhập!";
            return Json(result);
        }

        [HttpPost]
        public ActionResult RejectBillImport(long id)
        {
            bool result = billImportDAO.RejectBillImport(id);
            TempData["BillImport"] = "Đã hủy phiếu nhập!";
            return Json(result);
        }
    }
}