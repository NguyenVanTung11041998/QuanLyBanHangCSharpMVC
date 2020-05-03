using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Models.Constants;
using Models.DTO;
using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDAO
    {
        private QuanLyBanHangCSharpDbContext dbContext = new QuanLyBanHangCSharpDbContext();
        public PagedResultDto<Product> GetAllProductByKeyWord(string search, int currentPage, int pageSize)
        {
            IQueryable<Product> products = dbContext.Products.OrderByDescending(x => x.Id);
            if (!string.IsNullOrWhiteSpace(search))
                products = products.Where(x => x.Name.Contains(search.Trim()));
            int totalCount = products.Count();
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Product>(totalCount, products.ToList());
        }

        public List<ProductModel> GetAllProductImageByKeyWord(string search)
        {
            var query = dbContext.Products.Where(x => x.ProductStatus == ProductStatus.Active)
                                .WhereIf(!string.IsNullOrWhiteSpace(search), x => x.Name.Contains(search.Trim()))
                                .OrderByDescending(x => x.Id);
            var assets = dbContext.Assets;

            var result = from x in query
                        join a in assets on x.Id equals a.ProductId
                        group a by x into gr
                        select new ProductModel
                        {
                            Id = gr.Key.Id,
                            Name = gr.Key.Name,
                            Price = gr.Key.Price,
                            Image = new Image
                            {
                                Name = gr.FirstOrDefault().Name,
                                Path = gr.FirstOrDefault().Path
                            }
                        };
            return result.ToList();
        }

        public async Task<long> CreateProductAsync(ProductRequest productRequest)
        {
            var product = new Product
            {
                AmountProduct = productRequest.AmountProduct,
                CategoryId = productRequest.CategoryId,
                CreationTime = DateTime.Now,
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                ProducerId = productRequest.ProducerId,
                ProductStatus = ProductStatus.Active
            };
            dbContext.Products.Add(product);
            productRequest.Images.ForEach(x =>
            {
                var image = new Asset
                {
                    Name = x.Name,
                    ProductId = product.Id,
                    Path = x.Path
                };
                dbContext.Assets.Add(image);
            });
            await dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> EditProductAsync(ProductRequest productRequest)
        {
            var productUpdate = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == productRequest.Id);
            productUpdate.Name = productRequest.Name;
            productUpdate.CategoryId = productRequest.CategoryId;
            productUpdate.ProducerId = productRequest.ProducerId;
            productUpdate.Price = productRequest.Price;
            productUpdate.Description = productRequest.Description;
            var assets = await dbContext.Assets.Where(x => x.ProductId == productRequest.Id).ToListAsync();
            if (productRequest?.Images.Count > 0)
            {
                if (productRequest.Images[0] != null)
                    assets.ForEach(x => dbContext.Assets.Remove(x));
                productRequest.Images.ForEach(x =>
                {
                    if (x != null)
                    {
                        var image = new Asset
                        {
                            Name = x.Name,
                            Path = x.Path,
                            ProductId = productRequest.Id
                        };
                        dbContext.Assets.Add(image);
                    }
                });
            }
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductStatus> ChangeStatusAsync(long id)
        {
            var productDelete = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            productDelete.ProductStatus = productDelete.ProductStatus == ProductStatus.Active ? ProductStatus.Deactive : ProductStatus.Active;
            await dbContext.SaveChangesAsync();
            return productDelete.ProductStatus;
        }

        public async Task<ActionStatus> DeleteAsync(long id)
        {
            bool check = await dbContext.BillImportInfos.AnyAsync(x => x.ProductId == id) || await dbContext.BillInfos.AnyAsync(x => x.ProductId == id);
            var productDelete = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            var status = ActionStatus.DeleteSuccess;
            if (!check && productDelete != null)
            {
                var assets = await dbContext.Assets.Where(x => x.ProductId == id).ToListAsync();
                assets.ForEach(x => dbContext.Assets.Remove(x));
                dbContext.Products.Remove(productDelete);
            }
            else if (check && productDelete != null)
            {
                productDelete.ProductStatus = ProductStatus.Deactive;
                status = ActionStatus.ChangeStatus;
            }
            else
                status = ActionStatus.DeleteFail;
            await dbContext.SaveChangesAsync();
            return status;
        }

        public async Task<ProductRequest> GetProductByIdAsync(long id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return null;
            var asset = await dbContext.Assets.Where(x => x.ProductId == id).Select(x => new Image { Name = x.Name, Path = x.Path }).ToListAsync();
            return new ProductRequest
            {
                AmountProduct = product.AmountProduct,
                CategoryId = product.CategoryId,
                CreationTime = product.CreationTime,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                ProducerId = product.ProducerId,
                ProductStatus = product.ProductStatus,
                Id = product.Id,
                Images = asset
            };
        }

        public async Task<CategoryProducerDto> GetAllCategoryProducerActive()
        {
            var producers = await dbContext.Producers.Where(x => x.ProducerStatus == ProducerStatus.Active).Select(x => new ProducerDto { Id = x.Id, Name = x.Name }).ToListAsync();
            var categories = await dbContext.Categories.Where(x => x.CategoryStatus == CategoryStatus.Active).Select(x => new CategoryDto { Id = x.Id, Name = x.Name }).ToListAsync();
            return new CategoryProducerDto
            {
                Categories = categories,
                Producers = producers
            };
        }

        public async Task<ProductRequest> GetProductById(long id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.ProductStatus == ProductStatus.Active);
            if (product == null)
                return null;
            var assets = await dbContext.Assets.Where(x => x.ProductId == id).Select(x => new Image { Name = x.Name, Path = x.Path }).ToListAsync();
            return new ProductRequest
            {
                AmountProduct = product.AmountProduct,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Images = assets,
                CategoryId = product.CategoryId,
                ProducerId = product.ProducerId
            };
        }

        public async Task<ProductAndRelated> GetProductByIdAndRelated(long id)
        {
            var product = await GetProductById(id);
            if (product == null)
                return null;
            var queryProductRelated = await dbContext.Products.Where(x => x.CategoryId == product.CategoryId && x.Id != product.Id && x.ProductStatus == ProductStatus.Active)
                                                        .OrderByDescending(x => x.CreationTime)
                                                        .Take(3).ToListAsync();
            var productRelated = (from x in queryProductRelated
                                  join p in dbContext.Assets on x.Id equals p.ProductId
                                  group p by x into gr
                                  select new ProductModel
                                  {
                                      Id = gr.Key.Id,
                                      Name = gr.Key.Name,
                                      Price = gr.Key.Price,
                                      Image = gr.Select(x => new Image { Name = x.Name, Path = x.Path }).FirstOrDefault()
                                  }).ToList();
            return new ProductAndRelated { Product = product, ProductRelated = productRelated };
        }

        public List<ProductModel> GetNewProducts(int count = 3)
        {
            var products = (from product in dbContext.Products.Where(x => x.ProductStatus == ProductStatus.Active).OrderByDescending(x => x.Id).Take(count)
                            join asset in dbContext.Assets on product.Id equals asset.ProductId
                            group asset by product into gr
                            select new ProductModel
                            {
                                Id = gr.Key.Id,
                                Name = gr.Key.Name,
                                Price = gr.Key.Price,
                                Image = gr.Select(x => new Image
                                {
                                    Name = x.Name,
                                    Path = x.Path
                                }).FirstOrDefault()
                            }).ToList();
            return products;
        }

        public async Task<List<ProductModel>> GetAllProductByCategory(long categoryId, int pageSize = 9, int currentPage = 1)
        {
            var products = dbContext.Products.Where(x => x.ProductStatus == ProductStatus.Active && x.CategoryId == categoryId)
                                            .OrderByDescending(x => x.Id)
                                            .Skip((currentPage - 1) * pageSize)
                                            .Take(pageSize);
            var productResult = await (from product in products
                                       join asset in dbContext.Assets on product.Id equals asset.ProductId
                                       group asset by product into gr
                                       select new ProductModel
                                       {
                                           Id = gr.Key.Id,
                                           Name = gr.Key.Name,
                                           Price = gr.Key.Price,
                                           Image = gr.Select(x => new Image
                                           {
                                               Name = x.Name,
                                               Path = x.Path
                                           }).FirstOrDefault()
                                       }).ToListAsync();
            return productResult;
        }
    }
}
