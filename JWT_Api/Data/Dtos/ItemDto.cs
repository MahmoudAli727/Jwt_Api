using System.ComponentModel.DataAnnotations;

namespace JWT_Api.Data.Dtos
{
    public class ItemDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Notes { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}
