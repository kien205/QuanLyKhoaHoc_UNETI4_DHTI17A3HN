// Repository cho HoSoXetDuyetController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class HoSoXetDuyetRepository : IHoSoXetDuyetRepository
    {
        private readonly AppDbContext _context;

        public HoSoXetDuyetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<HoSoDangKyHoc> DanhSach, int TongBanGhi)> TimKiemAsync(
            string? tuKhoa,
            int? maChuongTrinh,
            string? trangThai,
            DateTime? tuNgay,
            DateTime? denNgay,
            string? sapXep,
            int trang,
            int pageSize)
        {
            var query = _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string tk = tuKhoa.Trim().ToLower();
                query = query.Where(h => (h.HocVien != null && h.HocVien.HoTen.ToLower().Contains(tk)) ||
                                         (h.HocVien != null && h.HocVien.Email != null &&
                                          h.HocVien.Email.ToLower().Contains(tk)) ||
                                         (h.LopHoc != null && h.LopHoc.TenLop.ToLower().Contains(tk)));
            }

            if (maChuongTrinh.HasValue)
            {
                query = query.Where(h => h.LopHoc != null && h.LopHoc.MaChuongTrinhDaoTao == maChuongTrinh.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(h => h.TrangThai == trangThai);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(h => h.NgayNop >= tuNgay.Value.Date);
            }
            if (denNgay.HasValue)
            {
                DateTime denNgayCuoiNgay = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(h => h.NgayNop <= denNgayCuoiNgay);
            }

            query = sapXep switch
            {
                "ngay_tang" => query.OrderBy(h => h.NgayNop),
                "ten_az" => query.OrderBy(h => h.HocVien != null ? h.HocVien.HoTen : string.Empty),
                "ten_za" => query.OrderByDescending(h => h.HocVien != null ? h.HocVien.HoTen : string.Empty),
                _ => query.OrderByDescending(h => h.NgayNop)
            };

            int tongBanGhi = await query.CountAsync();
            int tongSoTrang = (int)Math.Ceiling(tongBanGhi / (double)pageSize);
            if (trang < 1) trang = 1;
            if (tongSoTrang > 0 && trang > tongSoTrang) trang = tongSoTrang;

            var danhSach = await query
                .Skip((trang - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (danhSach, tongBanGhi);
        }

        public Task<List<ChuongTrinhDaoTao>> LayDanhSachChuongTrinhAsync() =>
            _context.ChuongTrinhDaoTaos.OrderBy(c => c.TenChuongTrinhDaoTao).ToListAsync();

        public Task<HoSoDangKyHoc?> LayChiTietHoSoAsync(int maHoSo) =>
            _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(h => h.MaHoSo == maHoSo);

        public Task<int> DemSoDaXepLopAsync(int maLop) =>
            _context.HoSoDangKyHocs
                .CountAsync(h => h.MaLop == maLop && h.TrangThai == TrangThaiHoSo.DuocXepLop);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
