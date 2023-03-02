using System.ComponentModel.DataAnnotations;

namespace WebAPI_Train4.Models
{
    public class CategoryModel
    {
        // Có model để người dùng nhập vào
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
