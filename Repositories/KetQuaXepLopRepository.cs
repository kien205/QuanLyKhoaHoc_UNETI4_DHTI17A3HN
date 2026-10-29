// Repository cho KetQuaXepLopController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class KetQuaXepLopRepository : IKetQuaXepLopRepository
    {
        private readonly AppDbContext _context;

        public KetQuaXepLopRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<KetQuaXepLop>> LayTatCaAsync() =>
            _context.KetQuaXepLops
                .Include(k => k.HoSoDangKyHoc!).ThenInclude(h => h.HocVien)
                .Include(k => k.HoSoDangKyHoc!).ThenInclude(h => h.LopHoc)
                .OrderByDescending(k => k.NgayCapNhat)
                .ToListAsync();

        public Task<HoSoDangKyHoc?> LayHoSoKemLopAsync(int? maHoSo) =>
            _context.HoSoDangKyHocs
                .Include(h => h.LopHoc)
                .FirstOrDefaultAsync(h => h.MaHoSo == maHoSo);

        public Task<bool> HoSoDaCoKetQuaAsync(int maHoSo) =>
            _context.KetQuaXepLops.AnyAsync(k => k.MaHoSo == maHoSo);

        public Task<int> DemSoDaXepLopAsync(int maLop) =>
            _context.HoSoDangKyHocs.CountAsync(h =>
                h.MaLop == maLop &&
                h.TrangThai == TrangThaiHoSo.DuocXepLop);

        public Task<List<HoSoDangKyHoc>> LayHoSoChoNhapKetQuaAsync() =>
            _context.HoSoDangKyHocs
                .AsNoTracking()
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                .Where(h => h.TrangThai == TrangThaiHoSo.DaKiemTraDauVao &&
                            h.KetQuaXepLop == null)
                .OrderBy(h => h.HocVien!.HoTen)
                .ToListAsync();

        public void Them(KetQuaXepLop ketQua) => _context.KetQuaXepLops.Add(ketQua);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
