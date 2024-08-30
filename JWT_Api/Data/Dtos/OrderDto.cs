using System.ComponentModel.DataAnnotations;

namespace JWT_Api.Data.Dtos
{
    public class OrderDto
    {
        public int orderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [MaxLength(100)]
        public string OrederName { get; set; }

        public ICollection<OrdersItemsDto> items { get; set; } = new List<OrdersItemsDto>();

    }
    public class OrdersItemsDto
    {
        [Required]
        public int itemId { get; set; }
        public string? itemName { get; set; }

        [Required]
        public double price { get; set; }

        public int quantity { get; set; }
    }

}
