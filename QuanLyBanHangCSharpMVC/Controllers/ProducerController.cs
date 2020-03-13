using Models.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class ProducerController : Controller
    {
        private ProducerDAO producerDAO = new ProducerDAO();
        [HttpGet]
        public async Task<ActionResult> Index(long id, int currentPage = 1, int pageSize = 9)
        {
            var products = await producerDAO.GetAllProductByProducerAsync(id, currentPage, pageSize);
            return View(products);
        }
    }
}