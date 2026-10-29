// Repository cho ThongKeController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class ThongKeRepository : IThongKeRepository
    {
        private readonly AppDbContext _context;

        public ThongKeRepository(AppDbContext context)
        {
            _context = context;
        }

        // ===== Dashboard =====
        public Task<int> DemChuongTrinhAsync() => _context.ChuongTrinhDaoTaos.CountAsync();

        public Task<int> DemLopAsync(string? trangThai = null) =>
            trangThai == null
                ? _context.LopHocs.CountAsync()
                : _context.LopHocs.CountAsync(l => l.TrangThai == trangThai);

        public Task<int> DemHocVienAsync() => _context.HocViens.CountAsync();

        public Task<int> DemHoSoAsync(string? trangThai = null) =>
            trangThai == null
                ? _context.HoSoDangKyHocs.CountAsync()
                : _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == trangThai);

        public Task<int> DemLichSapToiAsync(DateTime tuThoiDiem) =>
            _context.LichKiemTraDauVaos.CountAsync(l =>
                l.TrangThai == TrangThaiLich.DaLenLich && l.ThoiGianBatDau >= tuThoiDiem);

        // ===== Thống kê chi tiết =====
        public async Task<List<(string Nhan, int SoLuong)>> DemLopTheoChuongTrinhAsync()
        {
            var ds = await _context.ChuongTrinhDaoTaos
                .OrderBy(c => c.TenChuongTrinhDaoTao)
                .Select(c => new { Nhan = c.TenChuongTrinhDaoTao, SoLuong = c.LopHocs.Count() })
                .ToListAsync();
            return ds.Select(x => (x.Nhan, x.SoLuong)).ToList();
        }

        public async Task<List<(int MaLop, int Tong, int QuaDuyet, int DaXet)>> ThongKeHoSoTheoLopAsync(
            string[] trangThaiQuaDuyet, string[] trangThaiDaXetDuyet)
        {
            var ds = await _context.HoSoDangKyHocs
                .GroupBy(h => h.MaLop)
                .Select(g => new
                {
                    MaLop = g.Key,
                    Tong = g.Count(),
                    QuaDuyet = g.Count(h => trangThaiQuaDuyet.Contains(h.TrangThai)),
                    DaXet = g.Count(h => trangThaiDaXetDuyet.Contains(h.TrangThai))
                })
                .ToListAsync();
            return ds.Select(x => (x.MaLop, x.Tong, x.QuaDuyet, x.DaXet)).ToList();
        }

        public async Task<List<(int MaLop, string TenLop)>> LayDanhSachLopAsync()
        {
            var ds = await _context.LopHocs
                .OrderBy(l => l.TenLop)
                .Select(l => new { l.MaLop, l.TenLop })
                .ToListAsync();
            return ds.Select(x => (x.MaLop, x.TenLop)).ToList();
        }

        public async Task<List<(string Nhan, int SoLuong)>> DemHoSoTheoTrangThaiAsync()
        {
            var ds = await _context.HoSoDangKyHocs
                .GroupBy(h => h.TrangThai)
                .Select(g => new { Nhan = g.Key, SoLuong = g.Count() })
                .ToListAsync();
            return ds.Select(x => (x.Nhan, x.SoLuong)).ToList();
        }

        public async Task<List<(string Nhan, int SoLuong)>> DemXepLopTheoChuongTrinhAsync()
        {
            var ds = await _context.ChuongTrinhDaoTaos
                .OrderBy(c => c.TenChuongTrinhDaoTao)
                .Select(c => new
                {
                    Nhan = c.TenChuongTrinhDaoTao,
                    SoLuong = _context.HoSoDangKyHocs.Count(h =>
                        h.LopHoc!.MaChuongTrinhDaoTao == c.MaChuongTrinhDaoTao &&
                        h.TrangThai == TrangThaiHoSo.DuocXepLop)
                })
                .ToListAsync();
            return ds.Select(x => (x.Nhan, x.SoLuong)).ToList();
        }

        public async Task<List<(int Nam, int Thang, int SoLuong)>> DemHoSoTheoThangAsync()
        {
            var ds = await _context.HoSoDangKyHocs
                .GroupBy(h => new { h.NgayNop.Year, h.NgayNop.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, SoLuong = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();
            return ds.Select(x => (x.Year, x.Month, x.SoLuong)).ToList();
        }

        public async Task<List<(int Nam, int Thang, int SoLuong)>> DemLichTheoThangAsync()
        {
            var ds = await _context.LichKiemTraDauVaos
                .GroupBy(l => new { l.ThoiGianBatDau.Year, l.ThoiGianBatDau.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, SoLuong = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();
            return ds.Select(x => (x.Year, x.Month, x.SoLuong)).ToList();
        }
    }
}
