using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace WebAPI_Train4.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        // MUốn map với database thì tạo DbSet
        #region DbSet
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<HangHoa> hangHoas { get; set; } 
        public DbSet<Category> categories { get; set; }
        #endregion
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<DonHangChiTiet> donHangChiTiets { get; set; }
        public DbSet<DonHang> donHangs { get; set; }
        // Định nghĩa Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bien dich lenh add migration
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHang");

                // Khai báo khóa chính
                e.HasKey(dh => dh.MaDH);
                // UTC là lấy múi giờ thế giới
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");

                e.Property(dh => dh.NguoiNhan)
                .IsRequired()
                .HasMaxLength(100);
            });

            //Bien dich lenh add migration
            modelBuilder.Entity<DonHangChiTiet>(entity =>
            {
                entity.ToTable("ChiTietDonHang");
                entity.HasKey(entity => new { entity.MaDH, entity.MaHH });

                entity.HasOne(e => e.DonHang)
                .WithMany(e => e.DonHangChiTiet)
                .HasForeignKey(e => e.MaDH)
                .HasConstraintName("FK_DonHangChiTiet_DonHang");

                entity.HasOne(e => e.HangHoa)
                .WithMany(e => e.DonHangChiTiet)
                .HasForeignKey(e => e.MaHH)
                .HasConstraintName("FK_DonHangChiTiet_HangHoa");
            });
            //Bien dich lenh add-migration
            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}
