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
        public DbSet<HangHoa> hangHoas { get; set; } 
        public DbSet<Category> categories { get; set; }
        #endregion
    }
}
