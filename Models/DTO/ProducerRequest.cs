using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class ProducerRequest
    {
        public long Id { get; set; }
        [Display(Name = "Tên hãng")]
        [Required(ErrorMessage = "Bạn chưa nhập tên hãng!")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public Image Logo { get; set; }
    }
}
