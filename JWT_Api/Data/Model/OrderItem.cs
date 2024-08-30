using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JWT_Api.Data.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(orders))]
        public int OrderId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Order? orders { get; set; }

        [ForeignKey(nameof(items))]
        public int ItemId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Item? items { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
