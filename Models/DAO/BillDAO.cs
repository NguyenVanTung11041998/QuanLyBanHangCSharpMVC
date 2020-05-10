using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Models.DTO;
using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class BillDAO : BaseDAO
    {
        public async Task<long> CreateBill(Bill bill)
        {
            DBContext.Bills.Add(bill);
            await DBContext.SaveChangesAsync();
            return bill.Id;
        }

        public async Task<int> CreateBillInfo(List<BillInfo> billInfos)
        {
            DBContext.BillInfos.AddRange(billInfos);
            return await DBContext.SaveChangesAsync();
        }

        [Obsolete]
        public PagedResultDto<Bill> GetAllBill(DateTime? dateTime, int currentPage, int pageSize)
        {
            IQueryable<Bill> bills = DBContext.Bills.OrderByDescending(x => x.CreationTime);
            bills = bills.WhereIf(dateTime != null, x => EntityFunctions.TruncateTime(x.CreationTime) == EntityFunctions.TruncateTime(dateTime));
            int totalCount = bills.Count();
            bills = bills.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Bill>(totalCount, bills.ToList());
        }

        public async Task<BillDto> GetAllBillInfoInfoByBillId(long billId)
        {
            var bill = await DBContext.Bills.Where(x => x.Id == billId)
                                            .Select(x => new BillDto
                                            {
                                                Address = x.Address,
                                                BillStatus = x.BillStatus,
                                                CreationTime = x.CreationTime,
                                                Id = x.Id,
                                                Phone = x.Account.User.Phone,
                                                TotalPrice = x.TotalPrice,
                                                UserName = x.Account.User.Name,
                                                VAT = x.VAT,
                                                BillInfos = DBContext.BillInfos.Where(p => p.BillId == billId)
                                                                .Select(p => new BillInfoDto
                                                                {
                                                                    Id = p.Id,
                                                                    BillId = p.BillId,
                                                                    Price = p.Price,
                                                                    ProductId = p.ProductId,
                                                                    ProductName = p.Product.Name,
                                                                    QuantityPurchased = p.QuantityPurchased
                                                                }).ToList()
                                            }).FirstOrDefaultAsync();
            return bill;
        }

        public async Task<bool> RejectBill(long id)
        {
            var bill = await DBContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
            if (bill.BillStatus == BillStatus.Confirmed)
            {
                var billInfos = DBContext.BillInfos.Where(x => x.BillId == id);
                var products = await billInfos.Select(x => x.Product).ToListAsync();
                products.ForEach(x => x.AmountProduct += billInfos.FirstOrDefault(p => p.ProductId == x.Id).QuantityPurchased);
            }
            bill.BillStatus = BillStatus.Cancel;
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ConfirmBill(long id)
        {
            var bill = await DBContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
            bill.BillStatus = BillStatus.Confirmed;
            var billInfos = DBContext.BillInfos.Where(x => x.BillId == id);
            var products = await billInfos.Select(x => x.Product).ToListAsync();
            foreach (var item in products)
            {
                var productOrder = await billInfos.FirstOrDefaultAsync(p => p.ProductId == item.Id);
                if (item.AmountProduct < productOrder.QuantityPurchased)
                    return false;
                item.AmountProduct -= productOrder.QuantityPurchased;
            }
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> PayBill(long id)
        {
            var bill = await DBContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
            bill.BillStatus = BillStatus.AlreadyPaid;
            return await DBContext.SaveChangesAsync() > 0;
        }
    }
}
