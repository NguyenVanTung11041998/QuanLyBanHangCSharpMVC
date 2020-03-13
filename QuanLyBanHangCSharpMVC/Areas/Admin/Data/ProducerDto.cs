using Models.EntityFrameworkCore;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Data
{
    public class ProducerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ProducerStatus ProducerStatus { get; set; }
        public string Logo { get; set; }
    }
}