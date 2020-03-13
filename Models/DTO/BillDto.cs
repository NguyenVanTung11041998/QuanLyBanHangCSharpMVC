using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Models.DTO
{
    public class BillDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<BillInfoDto> BillInfos { get; set; }
        public BillStatus BillStatus { get; set; }
        public long TotalPrice { get; set; }
        public long VAT { get; set; }
    }
}
