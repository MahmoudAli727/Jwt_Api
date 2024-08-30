using System.ComponentModel.DataAnnotations;

namespace JWT_Api.Data.Model
{
    public class Order
    {
        [Key]
        public int id { get; set; }
        public DateTime CreatedDate { set; get; }

        public virtual ICollection<OrderItem>? ordersItems { get; set; }

    }
}
