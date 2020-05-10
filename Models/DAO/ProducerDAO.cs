using Abp.Application.Services.Dto;
using Models.Constants;
using Models.DTO;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProducerDAO : BaseDAO
    {
        public PagedResultDto<Producer> GetAllProducerByKeyWord(string search, int currentPage, int pageSize)
        {
            IQueryable<Producer> producers = DBContext.Producers.OrderByDescending(x => x.Id);
            if (!string.IsNullOrWhiteSpace(search))
                producers = producers.Where(x => x.Name.Contains(search.Trim()));
            int totalCount = producers.Count();
            producers = producers.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Producer>(totalCount, producers.ToList());
        }

        public async Task<long> CreateProducerAsync(ProducerRequest producerRequest)
        {
            var producer = new Producer
            {
                Addess = producerRequest.Address,
                Name = producerRequest.Name,
                Phone = producerRequest.Phone,
                ProducerStatus = ProducerStatus.Active
            };
            DBContext.Producers.Add(producer);
            if (producerRequest.Logo != null)
            {
                var logo = new Asset
                {
                    Name = producerRequest.Logo.Name,
                    Path = producerRequest.Logo.Path,
                    ProducerId = producer.Id
                };
                DBContext.Assets.Add(logo);
            }
            await DBContext.SaveChangesAsync();
            return producer.Id;
        }

        public async Task<bool> EditProducerAsync(ProducerRequest producerRequest)
        {
            var producer = await DBContext.Producers.FirstOrDefaultAsync(x => x.Id == producerRequest.Id);
            producer.Name = producerRequest.Name;
            producer.Phone = producerRequest.Phone;
            producer.Addess = producerRequest.Address;
            var assets = await DBContext.Assets.Where(x => x.ProducerId == producerRequest.Id).ToListAsync();
            assets.ForEach(x => DBContext.Assets.Remove(x));
            if (producerRequest.Logo != null)
            {
                var logo = new Asset
                {
                    Name = producerRequest.Logo.Name,
                    Path = producerRequest.Logo.Path,
                    ProducerId = producer.Id
                };
                DBContext.Assets.Add(logo);
            }
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<ProducerStatus> ChangeStatusAsync(long id)
        {
            var producerDelete = await DBContext.Producers.FirstOrDefaultAsync(x => x.Id == id);
            producerDelete.ProducerStatus = producerDelete.ProducerStatus == ProducerStatus.Active ? ProducerStatus.Deactive : ProducerStatus.Active;
            await DBContext.SaveChangesAsync();
            return producerDelete.ProducerStatus;
        }

        public async Task<ActionStatus> DeleteAsync(long id)
        {
            var check = await DBContext.Products.AnyAsync(x => x.ProducerId == id);
            var producerDelete = await DBContext.Producers.FirstOrDefaultAsync(x => x.Id == id);
            var status = ActionStatus.DeleteSuccess;
            if (!check && producerDelete != null)
            {
                var assets = await DBContext.Assets.Where(x => x.ProducerId == id).ToListAsync();
                assets.ForEach(x => DBContext.Assets.Remove(x));
                DBContext.Producers.Remove(producerDelete);
            }
            else if (check && producerDelete != null)
            {
                producerDelete.ProducerStatus = ProducerStatus.Deactive;
                status = ActionStatus.ChangeStatus;
            }
            else
                status = ActionStatus.DeleteFail;
            await DBContext.SaveChangesAsync();
            return status;
        }

        public async Task<ProducerRequest> GetProducerByIdAsync(long id)
        {
            var producer = await DBContext.Producers.FirstOrDefaultAsync(x => x.Id == id);
            if (producer == null)
                return null;
            var asset = await DBContext.Assets.FirstOrDefaultAsync(x => x.ProducerId == id);
            return new ProducerRequest
            {
                Address = producer.Addess,
                Name = producer.Name,
                Id = producer.Id,
                Phone = producer.Phone,
                Logo = asset != null ? new Image
                {
                    Name = asset?.Name,
                    Path = asset?.Path
                } : null
            };
        }

        public async Task<List<ProductModel>> GetAllProductByProducerAsync(long producerId, int currentPage, int pageSize)
        {
            var products = DBContext.Products.Where(x => x.ProductStatus == ProductStatus.Active && x.ProducerId == producerId)
                                            .OrderByDescending(x => x.Id)
                                            .Skip((currentPage - 1) * pageSize)
                                            .Take(pageSize);
            var productResult = await (from product in products
                                       join asset in DBContext.Assets on product.Id equals asset.ProductId
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

        public List<ProducerRequest> GetAllProducerActive()
        {
            var produces = DBContext.Producers.Where(x => x.ProducerStatus == ProducerStatus.Active);
            var images = DBContext.Assets;
            var result = (from x in produces
                          join p in images on x.Id equals p.ProducerId
                          group p by x into gr
                          select new ProducerRequest
                          {
                              Id = gr.Key.Id,
                              Address = gr.Key.Addess,
                              Name = gr.Key.Name,
                              Phone = gr.Key.Phone,
                              Logo = gr.Select(x => new Image { Name = x.Name, Path = x.Path }).FirstOrDefault()
                          }).ToList();
            return result;
        }

        public async Task<Image> GetImageProducer(long id)
        {
            var image = await DBContext.Assets.Where(x => x.ProducerId == id).Select(x => new Image { Name = x.Name, Path = x.Path }).FirstOrDefaultAsync();
            return image;
        }
    }
}
