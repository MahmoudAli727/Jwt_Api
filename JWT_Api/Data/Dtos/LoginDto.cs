using System.ComponentModel.DataAnnotations;

namespace JWT_Api.Data.Dtos
{
    public class LoginDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }
    }
}
