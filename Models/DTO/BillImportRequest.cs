using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Models.DTO
{
    public class BillImportRequest
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long TotalPrice { get; set; }
        public long UserId { get; set; }
        public BillImportType BillImportType { get; set; }
        public List<BillImportInfoRequest> BillImportInfos { get; set; }
    }
}
