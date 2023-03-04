using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Train4.Data
{
    public enum TinhTrangDonDatHang
    {
        New = 0, Payment = 1, Complete = 2, Cancel = -1
    }
    public class DonHang
    {
        [Key] // Thiết lập khóa chính, tự động tăng, nếu tên biến là Id thì mặc định sẽ được gán khóa chính
        
        public Guid MaDH { get; set; }
        
        public DateTime NgayDat { get; set; }
        
        public DateTime? NgayGiao { get; set; }
        
        [Required] // Not null
        [MaxLength(500)]
        public string NguoiNhan { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string DiaChiGiao { get; set; }
        
        public string SoDienThoai { get; set; }
        //[Range(0,double.MaxValue)]        
        //public double TongTien { get; set; }
           
        public TinhTrangDonDatHang TinhTrangDonHang { get; set; }
        public ICollection<DonHangChiTiet> DonHangChiTiet { get; set; }
        // Muốn trả về mảng rỗng thì
        public DonHang()
        {
            DonHangChiTiet = new List<DonHangChiTiet>();
        }
    }
}
