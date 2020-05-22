using Models.DAO;
using Models.DTO;
using QuanLyBanHangCSharpMVC.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private ProductDAO productDAO = new ProductDAO();
        [HttpGet]
        public ActionResult Index(string search, int currentPage = 1, int pageSize = 10)
        {
            var products = productDAO.GetAllProductByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(products.TotalCount, pageSize);
            return View(products.Items);
        }

        [HttpGet]
        public ActionResult ProductPartial(string search, int currentPage = 1, int pageSize = 10)
        {
            var products = productDAO.GetAllProductByKeyWord(search, currentPage, pageSize);
            ViewBag.PageNumber = Paginate.GetTotalPage(products.TotalCount, pageSize);
            return PartialView(products.Items);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var categoryProducers = await productDAO.GetAllCategoryProducerActive();
            ViewBag.Categories = new SelectList(categoryProducers.Categories, "Id", "Name");
            ViewBag.Producers = new SelectList(categoryProducers.Producers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(ProductRequest productRequest, List<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files?.Count > 0)
                {
                    string root = AppDomain.CurrentDomain.BaseDirectory;
                    string fileLocation = Path.Combine(root, "Content", "Images");
                    if (!Directory.Exists(fileLocation))
                        Directory.CreateDirectory(fileLocation);
                    productRequest.Images = new List<Image>();
                    foreach (var item in files)
                    {
                        if (item != null)
                        {
                            item.SaveAs($"{fileLocation}/{item.FileName}");
                            var image = new Image
                            {
                                Name = item.FileName,
                                Path = $"/Content/Images/{item.FileName}"
                            };
                            productRequest.Images.Add(image);
                        }
                    }
                }
                var result = await productDAO.CreateProductAsync(productRequest);
                if (result > 0)
                {
                    TempData["SaveProduct"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
            }
            return View(productRequest);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var product = await productDAO.GetProductByIdAsync(id);
            var categoryProducers = await productDAO.GetAllCategoryProducerActive();
            ViewBag.Categories = categoryProducers.Categories;
            ViewBag.Producers = categoryProducers.Producers;
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ProductRequest productRequest, List<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files?.Count > 0)
                {
                    string root = AppDomain.CurrentDomain.BaseDirectory;
                    string fileLocation = Path.Combine(root, "Content", "Images");
                    if (!Directory.Exists(fileLocation))
                        Directory.CreateDirectory(fileLocation);
                    productRequest.Images = new List<Image>();
                    foreach (var item in files)
                    {
                        if (item != null)
                        {
                            item.SaveAs($"{fileLocation}/{item.FileName}");
                            var image = new Image
                            {
                                Name = item.FileName,
                                Path = $"/Content/Images/{item.FileName}"
                            };
                            productRequest.Images.Add(image);
                        }
                    }
                }
                var result = await productDAO.EditProductAsync(productRequest);
                if (result)
                {
                    TempData["SaveProduct"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
            }
            return View(productRequest);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            var result = await productDAO.DeleteAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(long id)
        {
            var result = await productDAO.ChangeStatusAsync(id);
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(long id)
        {
            var product = await productDAO.GetProductById(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}