using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public long AmountProduct { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        [ForeignKey(nameof(Producer))]
        public long ProducerId { get; set; }
        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public virtual Category Category { get; set; }
        public virtual Producer Producer { get; set; }
    }

    public enum ProductStatus
    {
        Deactive = 0,
        Active = 1
    }
}
