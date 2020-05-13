using Models.DTO;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class StatisticalDAO : BaseDAO
    {
        private readonly List<int> monthOfYear = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public List<PriceDto> GetTotalPriceByYear(int year)
        {
            var prices = DBContext.Bills.Where(x => x.CreationTime.Year == year && x.BillStatus == BillStatus.AlreadyPaid).GroupBy(x => x.CreationTime.Month).Select(x => new PriceDto { Month = x.Key, TotalPrice = x.Sum(p => p.TotalPrice) }).ToList();
            var result = (from p in monthOfYear
                          join x in prices on p equals x.Month into gr
                          from x in gr.DefaultIfEmpty()
                          select new PriceDto
                          {
                              Month = p,
                              TotalPrice = x?.TotalPrice ?? 0
                          }).ToList();
            return result;
        }
    }
}
