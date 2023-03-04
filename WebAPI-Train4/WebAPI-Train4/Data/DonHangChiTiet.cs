using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Train4.Data
{
    public class DonHangChiTiet
    {

        public Guid MaHH { get; set; }
        public Guid MaDH { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }
        public DonHang DonHang { get; set; }// Tạo relationship
        public HangHoa HangHoa { get; set; }
    }
}
