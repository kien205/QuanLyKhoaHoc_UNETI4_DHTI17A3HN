// Repository cho DangKyHocController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class DangKyHocRepository : IDangKyHocRepository
    {
        private readonly AppDbContext _context;

        public DangKyHocRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<LopHoc>> LayLopDangMoDangKyAsync() =>
            _context.LopHocs
                .AsNoTracking()
                .Include(l => l.ChuongTrinhDaoTao)
                .Where(l => l.TrangThai == TrangThaiLop.DangMoDangKy)
                .OrderBy(l => l.HanDangKy)
                .ThenBy(l => l.TenLop)
                .ToListAsync();

        public Task<LopHoc?> LayLopKemChuongTrinhAsync(int maLop) =>
            _context.LopHocs
                .AsNoTracking()
                .Include(l => l.ChuongTrinhDaoTao)
                .SingleOrDefaultAsync(l => l.MaLop == maLop);

        public Task<bool> LopConNhanHoSoAsync(int maLop, DateTime homNay) =>
            _context.LopHocs.AnyAsync(l =>
                l.MaLop == maLop
                && l.TrangThai == TrangThaiLop.DangMoDangKy
                && l.NgayBatDauDangKy < homNay.AddDays(1)
                && l.HanDangKy >= homNay);

        public Task<List<int>> LayMaLopCoHoSoDangXuLyAsync(int maHocVien, int[] maLops, string[] trangThaiDangXuLy) =>
            _context.HoSoDangKyHocs
                .AsNoTracking()
                .Where(h => h.MaHocVien == maHocVien
                    && maLops.Contains(h.MaLop)
                    && trangThaiDangXuLy.Contains(h.TrangThai))
                .Select(h => h.MaLop)
                .ToListAsync();

        public Task<bool> CoHoSoDangXuLyAsync(int maHocVien, int maLop, string[] trangThaiDangXuLy) =>
            _context.HoSoDangKyHocs.AnyAsync(h =>
                h.MaHocVien == maHocVien
                && h.MaLop == maLop
                && trangThaiDangXuLy.Contains(h.TrangThai));

        public Task<List<HoSoDangKyHoc>> LayHoSoCuaHocVienAsync(int maHocVien) =>
            _context.HoSoDangKyHocs
                .AsNoTracking()
                .Where(h => h.MaHocVien == maHocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .Include(h => h.LichKiemTraDauVaos)
                .Include(h => h.KetQuaXepLop)
                .OrderByDescending(h => h.NgayNop)
                .ToListAsync();

        public Task<HoSoDangKyHoc?> LayHoSoCuaHocVienDeHuyAsync(int maHoSo, int maHocVien) =>
            _context.HoSoDangKyHocs
                .Include(h => h.LichKiemTraDauVaos)
                .Include(h => h.KetQuaXepLop)
                .SingleOrDefaultAsync(h => h.MaHoSo == maHoSo && h.MaHocVien == maHocVien);

        public void ThemHoSo(HoSoDangKyHoc hoSo) => _context.HoSoDangKyHocs.Add(hoSo);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
