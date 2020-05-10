using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Models.DTO;
using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class BillImportDAO
    {
        private readonly QuanLyBanHangCSharpDbContext dbContext = new QuanLyBanHangCSharpDbContext();
        public PagedResultDto<BillImport> GetAllBillImport(DateTime? dateTime, int currentPage, int pageSize)
        {
            IQueryable<BillImport> bills = dbContext.BillImports.OrderByDescending(x => x.CreationTime);
            bills = bills.WhereIf(dateTime != null, x => DbFunctions.TruncateTime(x.CreationTime) == DbFunctions.TruncateTime(dateTime));
            int totalCount = bills.Count();
            bills = bills.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<BillImport>(totalCount, bills.ToList());
        }

        public async Task<List<Product>> GetAllProductActive()
        {
            var products = await dbContext.Products.Where(x => x.ProductStatus == ProductStatus.Active).ToListAsync();
            return products;
        }

        public async Task<long> CreateBillImportAsync(BillImportRequest billImportRequest)
        {
            var billImport = new BillImport
            {
                UserId = billImportRequest.UserId,
                TotalPrice = billImportRequest.TotalPrice,
                BillImportType = billImportRequest.BillImportType,
                CreationTime = billImportRequest.CreationTime
            };
            dbContext.BillImports.Add(billImport);
            await dbContext.SaveChangesAsync();
            return billImport.Id;
        }

        public async Task<bool> CreateBillImportInfoAsync(List<BillImportInfo> billImportInfos, long id)
        {
            var billImport = dbContext.BillImports.FirstOrDefault(x => x.Id == id);
            billImport.TotalPrice += billImportInfos.Sum(x => x.Price * x.NumberImport);
            dbContext.BillImportInfos.AddRange(billImportInfos);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<BillImport> GetAllBillImportInfoByBillImportId(long billId)
        {
            return await dbContext.BillImports.FirstOrDefaultAsync(x => x.Id == billId);
        }

        public bool AcceptBillImport(long id)
        {
            var billImport = dbContext.BillImports.FirstOrDefault(x => x.Id == id);
            billImport.BillImportType = BillImportType.Confirmed;
            var billImportInfos = dbContext.BillImportInfos.Where(x => x.BillImportId == id).ToList();
            foreach (var item in billImportInfos)
            {
                var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                product.AmountProduct += item.NumberImport;
            }
            return dbContext.SaveChanges() > 0;
        }

        public bool RejectBillImport(long id)
        {
            var billImport = dbContext.BillImports.FirstOrDefault(x => x.Id == id);
            billImport.BillImportType = BillImportType.Cancel;
            return dbContext.SaveChanges() > 0;
        }

        public async Task<BillImport> GetBillImportByIdAsync(long id)
        {
            return await dbContext.BillImports.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
