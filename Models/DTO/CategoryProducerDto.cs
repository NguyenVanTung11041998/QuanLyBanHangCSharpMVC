using System.Collections.Generic;

namespace Models.DTO
{
    public class CategoryProducerDto
    {
        public List<CategoryDto> Categories { get; set; }
        public List<ProducerDto> Producers { get; set; }
    }
}
