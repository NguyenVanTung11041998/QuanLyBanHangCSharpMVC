using Models.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class ProductRequest
    {
        public long Id { get; set; }
        [Display(Name = "Ten sản phẩm")]
        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm!")]
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public long AmountProduct { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Bạn chưa nhập đơn giá!")]
        public long Price { get; set; }
        [Display(Name = "Hãng sản xuất")]
        public long ProducerId { get; set; }
        [Display(Name = "Danh mục")]
        public long CategoryId { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public List<Image> Images { get; set; }
    }
}
