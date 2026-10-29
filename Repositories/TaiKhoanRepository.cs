// Repository cho TaiKhoanController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly AppDbContext _db;

        public TaiKhoanRepository(AppDbContext db) => _db = db;

        public TaiKhoan? LayTheoTenDangNhap(string tenDangNhap) =>
            _db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == tenDangNhap);

        public Task<List<TaiKhoan>> TimKiemAsync(string? tuKhoa, string? vaiTro)
        {
            var q = _db.TaiKhoans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                q = q.Where(t => t.TenDangNhap.Contains(tuKhoa)
                              || t.HoTen.Contains(tuKhoa)
                              || t.Email.Contains(tuKhoa));
            }
            if (!string.IsNullOrWhiteSpace(vaiTro))
                q = q.Where(t => t.VaiTro == vaiTro);

            return q.OrderBy(t => t.TenDangNhap).ToListAsync();
        }

        public Task<bool> TenDangNhapTonTaiAsync(string tenDangNhap) =>
            _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == tenDangNhap);

        public async Task<TaiKhoan?> TimTheoMaAsync(int maTaiKhoan) =>
            await _db.TaiKhoans.FindAsync(maTaiKhoan);

        public Task<HocVien?> LayHocVienTheoTaiKhoanAsync(int maTaiKhoan, bool theoDoiThayDoi = false)
        {
            IQueryable<HocVien> hocViens = _db.HocViens;
            if (!theoDoiThayDoi)
                hocViens = hocViens.AsNoTracking();

            return hocViens.SingleOrDefaultAsync(h => h.MaTaiKhoan == maTaiKhoan);
        }

        public void Them(TaiKhoan taiKhoan) => _db.TaiKhoans.Add(taiKhoan);

        public void ThemHocVien(HocVien hocVien) => _db.HocViens.Add(hocVien);

        public Task LuuThayDoiAsync() => _db.SaveChangesAsync();
    }
}
