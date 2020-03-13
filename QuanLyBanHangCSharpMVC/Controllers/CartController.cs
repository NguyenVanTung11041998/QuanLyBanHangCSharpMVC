using Models.DAO;
using Models.EntityFrameworkCore;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class CartController : BaseController
    {
        private ProductDAO productDAO = new ProductDAO();
        private CartDAO cardDAO = new CartDAO();
        private BillDAO billDAO = new BillDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            var cards = await cardDAO.GetAllCartByAccount(id);
            ViewBag.ToTal = cards.Sum(x => x.QuantityPurchased);
            ViewBag.TotalPrice = cards.Sum(x => x.Price);
            return View(cards);
        }

        [HttpPost]
        public async Task<ActionResult> Create(long productId)
        {
            var accountId = ((Account)Session[Constant.UserCustomerSession]).Id;
            var product = await productDAO.GetProductById(productId);
            var cart = new Cart
            {
                AccountId = accountId,
                ProductId = productId,
                Price = product.Price,
                QuantityPurchased = 1
            };
            int result = await cardDAO.CreateCart(cart);
            if (result > 0)
                TempData["SaveCart"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(long id, long productId, long quantity)
        {
            var product = await productDAO.GetProductById(productId);
            int result = await cardDAO.EditCart(id, quantity, product.Price);
            if (result > 0)
                TempData["SaveCart"] = "Cập nhật số lượng thành công!";
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            int result = await cardDAO.DeleteCart(id);
            if (result > 0)
                TempData["DeleteCart"] = "Xóa sản phẩm thành công!";
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Order()
        {
            var account = (Account)Session[Constant.UserCustomerSession];
            var productOrders = await cardDAO.GetAllProductOrder(account.Id);
            if (productOrders == null)
                return Json(0);
            long totalPrice = productOrders.Sum(x => x.Price * x.QuantityPurchased);
            var bill = new Bill
            {
                Address = account.User.Address,
                CreationTime = DateTime.Now,
                BillStatus = BillStatus.AwaitingApproval,
                TotalPrice = totalPrice,
                VAT = (long)(totalPrice * 0.1),
                AccountId = account.Id
            };
            bill.Id = await billDAO.CreateBill(bill);
            var billInfos = productOrders.Select(x => new BillInfo
            {
                BillId = bill.Id,
                Price = x.Price,
                ProductId = x.ProductId,
                QuantityPurchased = x.QuantityPurchased
            }).ToList();
            int res = await billDAO.CreateBillInfo(billInfos);
            if (res > 0)
                TempData["Order"] = "Đơn hàng đã được thành công!";
            return Json(res);
        }
    }
}