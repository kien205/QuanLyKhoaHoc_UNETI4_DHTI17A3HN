using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data
{
    public class DuLieuMau
    {
        public static void KhoiTao(AppDbContext db)
        {
            db.Database.Migrate();
            if (db.TaiKhoans.Any()) return;

            db.TaiKhoans.AddRange(
                new TaiKhoan { TenDangNhap = "admin", MatKhau = "Admin@123", HoTen = "Quản trị viên", Email = "admin@uneti.edu.vn", VaiTro = VaiTroTaiKhoan.Admin },
                new TaiKhoan { TenDangNhap = "nhanvien1", MatKhau = "Nv@12345", HoTen = "Nguyễn Thị Lan", Email = "lan@uneti.edu.vn", VaiTro = VaiTroTaiKhoan.NhanVienDaoTao },
                new TaiKhoan { TenDangNhap = "nhanvien2", MatKhau = "Nv@12345", HoTen = "Trần Văn Minh", Email = "minh@uneti.edu.vn", VaiTro = VaiTroTaiKhoan.NhanVienDaoTao },
                new TaiKhoan { TenDangNhap = "khoa_test", MatKhau = "Hv@12345", HoTen = "Tài khoản bị khóa", Email = "khoa@uneti.edu.vn", VaiTro = VaiTroTaiKhoan.HocVien, TrangThai = false }
            );

            for (int i = 1; i <= 30; i++)
            {
                var tk = new TaiKhoan
                {
                    TenDangNhap = $"hocvien{i:00}",
                    MatKhau = "Hv@12345",
                    HoTen = $"Học viên {i:00}",
                    Email = $"hocvien{i:00}@gmail.com",
                    VaiTro = VaiTroTaiKhoan.HocVien
                };
                db.TaiKhoans.Add(tk);
                db.HocViens.Add(new HocVien
                {
                    TaiKhoan = tk,
                    HoTen = tk.HoTen,
                    NgaySinh = new DateTime(2000, 1, 1).AddDays(i * 30),
                    GioiTinh = i % 2 == 0 ? "Nam" : "Nữ",
                    SoDienThoai = $"09000000{i:00}",
                    Email = tk.Email,
                    TrinhDoHienTai = i % 3 == 0 ? "Trung cấp" : "Sơ cấp",
                    SoThangDaHoc = i % 12
                });
            }
            db.SaveChanges();
        }
    }
}
