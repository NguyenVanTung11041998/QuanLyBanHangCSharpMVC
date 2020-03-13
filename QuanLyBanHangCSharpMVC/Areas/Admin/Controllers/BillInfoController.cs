using Models.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class BillInfoController : BaseController
    {
        private BillDAO billDAO = new BillDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            var bill = await billDAO.GetAllBillInfoInfoByBillId(id);
            return View(bill);
        }
    }
}