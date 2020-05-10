using Models.DAO;
using Models.DTO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class BillImportController : BaseController
    {
        private BillImportDAO billImportDAO = new BillImportDAO();
        [HttpGet]
        public ActionResult Index(DateTime? dateTime, int currentPage = 1, int pageSize = 20)
        {
            var bills = billImportDAO.GetAllBillImport(dateTime, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(bills.TotalCount, pageSize);
            return View(bills.Items);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult BillImportPartial(DateTime? dateTime, int currentPage = 1, int pageSize = 20)
        {
            var bills = billImportDAO.GetAllBillImport(dateTime, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(bills.TotalCount, pageSize);
            return PartialView(bills.Items);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var products = await billImportDAO.GetAllProductActive();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection f)
        {
            var userId = ((Account)Session[Constant.UserAdminSession]).UserId;
            var billImport = new BillImportRequest
            {
                BillImportType = BillImportType.AwaitingApproval,
                CreationTime = DateTime.Now,
                TotalPrice = 0,
                UserId = userId
            };
            billImport.Id = await billImportDAO.CreateBillImportAsync(billImport);
            var billImportInfos = new List<BillImportInfo>();
            int count = int.Parse(Request.Cookies["RowCount"].Value);
            for (int i = 0; i < count; i++)
            {
                long productId = long.Parse(f[$"MaSP_{i}"]);
                long price = long.Parse(f[$"DonGiaNhap_{i}"]);
                int numberImport = int.Parse(f[$"SoLuongNhap_{i}"]);
                billImportInfos.Add(new BillImportInfo { BillImportId = billImport.Id, Price = price, ProductId = productId, NumberImport = numberImport });
            }
            bool result = await billImportDAO.CreateBillImportInfoAsync(billImportInfos, billImport.Id);
            if(result)
            {
                TempData["SaveBillImport"] = "Tạo phiếu nhập hàng thành công!";
                return RedirectToAction("Index");
            }
            return View("Create");
        }
    }
}