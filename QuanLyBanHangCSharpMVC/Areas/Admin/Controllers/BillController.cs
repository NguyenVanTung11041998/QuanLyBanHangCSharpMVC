using Models.DAO;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private BillDAO billDAO = new BillDAO();
        [HttpGet]
        [Obsolete]
        public ActionResult Index(DateTime? dateTime, int currentPage = 1, int pageSize = 20)
        {
            var bills = billDAO.GetAllBill(dateTime, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(bills.TotalCount, pageSize);
            return View(bills.Items);
        }

        [HttpGet]
        [Obsolete]
        [ChildActionOnly]
        public ActionResult BillPartial(DateTime? dateTime, int currentPage = 1, int pageSize = 20)
        {
            var bills = billDAO.GetAllBill(dateTime, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(bills.TotalCount, pageSize);
            return PartialView(bills.Items);
        }

        [HttpPost]
        public async Task<ActionResult> RejectBill(long id)
        {
            bool result = await billDAO.RejectBill(id);
            if (result)
                TempData["BillStatus"] = "Hủy phiếu thành công!";
            return Json(result ? 1 : 0);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmBill(long id)
        {
            bool result = await billDAO.ConfirmBill(id);
            if (result)
                TempData["BillStatus"] = "Bạn đã chuyển trạng thái hóa đơn thành công!";
            else
                TempData["BillStatus"] = "Không đủ hàng! Xin kiểm tra lại!";
            return Json(result ? 1 : 0);
        }

        [HttpPost]
        public async Task<ActionResult> PayBill(long id)
        {
            bool result = await billDAO.PayBill(id);
            if (result)
                TempData["BillStatus"] = "Thanh toán thành công!";
            return Json(result ? 1 : 0);
        }
    }
}