// Repository cho ChuongTrinhDaoTaoController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class ChuongTrinhDaoTaoRepository : IChuongTrinhDaoTaoRepository
    {
        private readonly AppDbContext _context;

        public ChuongTrinhDaoTaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<ChuongTrinhDaoTao>> LayTatCaKemLopHocAsync() =>
            _context.ChuongTrinhDaoTaos
                .Include(c => c.LopHocs)
                .OrderBy(c => c.TenChuongTrinhDaoTao)
                .ToListAsync();

        public Task<ChuongTrinhDaoTao?> LayChiTietKemLopHocAsync(int maChuongTrinh) =>
            _context.ChuongTrinhDaoTaos
                .Include(c => c.LopHocs)
                .FirstOrDefaultAsync(m => m.MaChuongTrinhDaoTao == maChuongTrinh);

        public async Task<ChuongTrinhDaoTao?> TimTheoMaAsync(int maChuongTrinh) =>
            await _context.ChuongTrinhDaoTaos.FindAsync(maChuongTrinh);

        public Task<bool> TrungTenAsync(string tenChuongTrinh, int? boQuaMa = null) =>
            _context.ChuongTrinhDaoTaos.AnyAsync(c =>
                c.TenChuongTrinhDaoTao == tenChuongTrinh &&
                (boQuaMa == null || c.MaChuongTrinhDaoTao != boQuaMa));

        public Task<bool> CoLopHocAsync(int maChuongTrinh) =>
            _context.LopHocs.AnyAsync(l => l.MaChuongTrinhDaoTao == maChuongTrinh);

        public void Them(ChuongTrinhDaoTao ctdt) => _context.ChuongTrinhDaoTaos.Add(ctdt);

        public async Task<bool> CapNhatAsync(ChuongTrinhDaoTao ctdt)
        {
            try
            {
                _context.Update(ctdt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ChuongTrinhDaoTaos
                        .AnyAsync(e => e.MaChuongTrinhDaoTao == ctdt.MaChuongTrinhDaoTao))
                    return false;
                throw;
            }
        }

        public void Xoa(ChuongTrinhDaoTao ctdt) => _context.ChuongTrinhDaoTaos.Remove(ctdt);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
