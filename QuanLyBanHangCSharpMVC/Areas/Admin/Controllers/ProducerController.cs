using Models.DAO;
using Models.DTO;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class ProducerController : BaseController
    {
        private ProducerDAO producerDAO = new ProducerDAO();
        [HttpGet]
        public ActionResult Index(string search, int currentPage = 1, int pageSize = 20)
        {
            var produces = producerDAO.GetAllProducerByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(produces.TotalCount, pageSize);
            return View(produces.Items);
        }

        [HttpGet]
        public ActionResult ProducerPartial(string search, int currentPage = 1, int pageSize = 20)
        {
            var produces = producerDAO.GetAllProducerByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(produces.TotalCount, pageSize);
            return PartialView(produces.Items);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Data.ProducerDto input, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var producerRequest = new ProducerRequest
                {
                    Address = input.Address,
                    Name = input.Name,
                    Phone = input.Phone
                };
                if (file != null)
                {
                    string root = AppDomain.CurrentDomain.BaseDirectory;
                    string fileLocation = Path.Combine(root, "Content", "Images");
                    if (!Directory.Exists(fileLocation))
                        Directory.CreateDirectory(fileLocation);
                    file.SaveAs($"{fileLocation}/{file.FileName}");
                    producerRequest.Logo = new Image
                    {
                        Name = file.FileName,
                        Path = $"/Content/Images/{file.FileName}"
                    };
                }
                input.Id = await producerDAO.CreateProducerAsync(producerRequest);
                if (input.Id > 0)
                {
                    TempData["SaveProducer"] = "Thêm hãng sản xuất thành công!";
                    return RedirectToAction("Index", "Producer");
                }
            }
            return View(input);
        }

        public async Task<ActionResult> Edit(long id)
        {
            var producerEdit = await producerDAO.GetProducerByIdAsync(id);
            if (producerEdit == null)
            {
                TempData["EditProducer"] = "Không tồn tại nhà sản xuất!";
                return View("Index");
            }
            var producer = new Data.ProducerDto
            {
                Address = producerEdit.Address,
                Id = producerEdit.Id,
                Logo = producerEdit.Logo?.Path,
                Name = producerEdit.Name,
                Phone = producerEdit.Phone
            };
            return View(producer);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Data.ProducerDto input, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var producerRequest = new ProducerRequest
                {
                    Id = input.Id,
                    Address = input.Address,
                    Name = input.Name,
                    Phone = input.Phone
                };
                if (file != null)
                {
                    string root = AppDomain.CurrentDomain.BaseDirectory;
                    string fileLocation = Path.Combine(root, "Content", "Images");
                    if (!Directory.Exists(fileLocation))
                        Directory.CreateDirectory(fileLocation);
                    file.SaveAs($"{fileLocation}/{file.FileName}");
                    producerRequest.Logo = new Image
                    {
                        Name = file.FileName,
                        Path = $"/Content/Images/{file.FileName}"
                    };
                }
                bool result = await producerDAO.EditProducerAsync(producerRequest);
                if (result)
                    TempData["SaveProducer"] = "Cập nhật hãng sản xuất thành công!";
                return RedirectToAction("Index", "Producer");
            }
            input.Logo = (await producerDAO.GetImageProducer(input.Id))?.Path;
            return View(input);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            var result = await producerDAO.DeleteAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(long id)
        {
            var result = await producerDAO.ChangeStatusAsync(id);
            return Json(result);
        }
    }
}