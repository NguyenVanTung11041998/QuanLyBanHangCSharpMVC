using Models.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCSharpMVC.Areas.Admin.Data
{
    public class ProducerDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống!")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ProducerStatus ProducerStatus { get; set; }
        public string Logo { get; set; }
    }
}