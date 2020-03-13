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
            var billImportInfos = await billImportDAO.GetAllBillImportInfoByBillImportId(id);
            var billImport = await billImportDAO.GetBillImportByIdAsync(id);
            if (billImport == null)
                return RedirectToAction("Index", "BillImport");
            ViewBag.BillImport = billImport;
            return View(billImportInfos);
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