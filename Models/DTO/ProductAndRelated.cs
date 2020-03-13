using System.Collections.Generic;

namespace Models.DTO
{
    public class ProductAndRelated
    {
        public ProductRequest Product { get; set; }
        public List<ProductModel> ProductRelated { get; set; }
    }
}
