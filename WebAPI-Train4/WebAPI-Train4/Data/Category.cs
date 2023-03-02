using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Train4.Data
{
    [Table("Categories")]
    public class Category
    {
        // Mã tự tăng do có identity
        // Do Tên không kết thúc bằng chữ ID nên không biết được set khóa chính
        [Key] 
        public int MaLoai { get; set; }

        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
        public virtual ICollection<HangHoa> HangHoa { get; set;}
    }
}
