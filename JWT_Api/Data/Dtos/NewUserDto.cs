using System.ComponentModel.DataAnnotations;

namespace JWT_Api.Data.Dtos
{
    public class NewUserDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

        public string? phoneNumber { get; set; }
    }
}
