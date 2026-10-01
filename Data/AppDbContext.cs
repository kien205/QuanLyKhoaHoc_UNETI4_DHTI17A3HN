using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
        public DbSet<ChuongTrinhDaoTao> ChuongTrinhDaoTaos => Set<ChuongTrinhDaoTao>();
        public DbSet<LopHoc> LopHocs => Set<LopHoc>();
        public DbSet<HocVien> HocViens => Set<HocVien>();
        public DbSet<HoSoDangKyHoc> HoSoDangKyHocs => Set<HoSoDangKyHoc>();
        public DbSet<LichKiemTraDauVao> LichKiemTraDauVaos => Set<LichKiemTraDauVao>();
        public DbSet<KetQuaXepLop> KetQuaXepLops => Set<KetQuaXepLop>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChuongTrinhDaoTao>().HasIndex(c => c.TenChuongTrinhDaoTao).IsUnique();

      
            modelBuilder.Entity<TaiKhoan>()
              .HasOne(t => t.HocVien).WithOne(h => h.TaiKhoan)
              .HasForeignKey<HocVien>(h => h.MaTaiKhoan);

            
            modelBuilder.Entity<HoSoDangKyHoc>()
              .HasOne(h => h.KetQuaXepLop).WithOne(k => k.HoSoDangKyHoc)
              .HasForeignKey<KetQuaXepLop>(k => k.MaHoSo);

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
