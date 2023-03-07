using System.ComponentModel.DataAnnotations;

namespace WebAPI_Train4.Models
{
    public class LoginModel
    {
        // Validation
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
