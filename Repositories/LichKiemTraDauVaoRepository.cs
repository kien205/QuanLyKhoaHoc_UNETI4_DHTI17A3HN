// Repository cho LichKiemTraDauVaoController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class LichKiemTraDauVaoRepository : ILichKiemTraDauVaoRepository
    {
        private readonly AppDbContext _context;

        public LichKiemTraDauVaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<LichKiemTraDauVao>> LayTatCaAsync() =>
            _context.LichKiemTraDauVaos
                .Include(l => l.HoSoDangKyHoc!).ThenInclude(h => h.HocVien)
                .Include(l => l.HoSoDangKyHoc!).ThenInclude(h => h.LopHoc)
                .OrderByDescending(l => l.ThoiGianBatDau)
                .ToListAsync();

        public Task<LichKiemTraDauVao?> LayLichKemHoSoAsync(int maLich) =>
            _context.LichKiemTraDauVaos
                .Include(l => l.HoSoDangKyHoc)
                .FirstOrDefaultAsync(l => l.MaLichKiemTraDauVao == maLich);

        public Task<HoSoDangKyHoc?> LayHoSoAsync(int? maHoSo) =>
            _context.HoSoDangKyHocs.FirstOrDefaultAsync(h => h.MaHoSo == maHoSo);

        public Task<bool> HoSoCoLichHieuLucAsync(int maHoSo) =>
            _context.LichKiemTraDauVaos.AnyAsync(l =>
                l.MaHoSo == maHoSo &&
                l.TrangThai != TrangThaiLich.DaHuy);

        public Task<bool> TrungLichHocVienAsync(int maHocVien, DateTime batDau, DateTime ketThuc) =>
            _context.LichKiemTraDauVaos.AnyAsync(l =>
                l.TrangThai != TrangThaiLich.DaHuy &&
                l.HoSoDangKyHoc!.MaHocVien == maHocVien &&
                l.ThoiGianBatDau < ketThuc &&
                l.ThoiGianKetThuc > batDau);

        public Task<bool> TrungLichGiaoVienAsync(string giaoVien, DateTime batDau, DateTime ketThuc) =>
            _context.LichKiemTraDauVaos.AnyAsync(l =>
                l.TrangThai != TrangThaiLich.DaHuy &&
                l.GiaoVienKiemTra == giaoVien &&
                l.ThoiGianBatDau < ketThuc &&
                l.ThoiGianKetThuc > batDau);

        public Task<List<HoSoDangKyHoc>> LayHoSoChoLapLichAsync() =>
            _context.HoSoDangKyHocs
                .AsNoTracking()
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                .Where(h => h.TrangThai == TrangThaiHoSo.DuDieuKien &&
                            !h.LichKiemTraDauVaos.Any(l => l.TrangThai != TrangThaiLich.DaHuy))
                .OrderBy(h => h.HocVien!.HoTen)
                .ToListAsync();

        public void Them(LichKiemTraDauVao lich) => _context.LichKiemTraDauVaos.Add(lich);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
