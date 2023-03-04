using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Train4.Data
{
    [Table("HangHoa")] // Định nghĩa chính xác tên table của nó là gì
    public class HangHoa
    {
        [Key] // Chỉ định MaHH là khóa chính
        public Guid MaHH { get; set; }

        [Required]// Bắt buộc phải nhập
        [MaxLength(100)] // Ràng buộc số ký tự nhập vào
        public string TenHH { get; set; }
        public string MoTa { get; set; } // Không nói gì hết thì được null, nvarchar max
        
        [Range(0,double.MaxValue)]     // Từ 0 và đến max, mặc định là 0
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        // dấu chấm hỏi dùng để biểu thị có thể có có thể không
        // vì nguyên tắc của khóa ngoại là có thể có có thể không
        public int? MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public Category Category { get; set; }
        
        public ICollection<DonHangChiTiet> DonHangChiTiet { get; set; }
        public HangHoa()
        {
            DonHangChiTiet=new HashSet<DonHangChiTiet>();
        }
    }
}
