// Repository cho LopHocController: chứa toàn bộ truy vấn CSDL tách ra từ controller.
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class LopHocRepository : ILopHocRepository
    {
        private readonly AppDbContext _context;

        public LopHocRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<LopHoc> DanhSach, int TongBanGhi)> TimKiemAsync(
            string? searchString,
            int? maChuongTrinhDaoTao,
            string? trinhDoDauVao,
            string? trangThai,
            double? diemDauVaoTu,
            string? hanDangKyFilter,
            string? sortOrder,
            int page,
            int pageSize)
        {
            IQueryable<LopHoc> query = _context.LopHocs
                .Include(x => x.ChuongTrinhDaoTao);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    x.TenLop.Contains(searchString) ||
                    (x.ChuongTrinhDaoTao != null &&
                     x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao.Contains(searchString)));
            }

            if (maChuongTrinhDaoTao.HasValue)
            {
                query = query.Where(x =>
                    x.MaChuongTrinhDaoTao == maChuongTrinhDaoTao.Value);
            }

            if (!string.IsNullOrWhiteSpace(trinhDoDauVao))
            {
                query = query.Where(x =>
                    x.TrinhDoDauVao == trinhDoDauVao);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(x =>
                    x.TrangThai == trangThai);
            }

            if (diemDauVaoTu.HasValue)
            {
                query = query.Where(x =>
                    x.DiemDauVao >= diemDauVaoTu.Value);
            }

            if (hanDangKyFilter == "con_han")
            {
                query = query.Where(x =>
                    x.HanDangKy >= DateTime.Now);
            }
            else if (hanDangKyFilter == "het_han")
            {
                query = query.Where(x =>
                    x.HanDangKy < DateTime.Now);
            }

            query = sortOrder switch
            {
                "name_desc" =>
                    query.OrderByDescending(x => x.TenLop),

                "program_asc" =>
                    query.OrderBy(x =>
                        x.ChuongTrinhDaoTao != null
                            ? x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao
                            : ""),

                "program_desc" =>
                    query.OrderByDescending(x =>
                        x.ChuongTrinhDaoTao != null
                            ? x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao
                            : ""),

                "size_asc" =>
                    query.OrderBy(x => x.SiSoToiDa),

                "size_desc" =>
                    query.OrderByDescending(x => x.SiSoToiDa),

                "score_asc" =>
                    query.OrderBy(x => x.DiemDauVao),

                "score_desc" =>
                    query.OrderByDescending(x => x.DiemDauVao),

                "date_asc" =>
                    query.OrderBy(x => x.HanDangKy),

                "date_desc" =>
                    query.OrderByDescending(x => x.HanDangKy),

                _ =>
                    query.OrderBy(x => x.TenLop)
            };

            int totalItems = await query.CountAsync();

            int totalPages = totalItems == 0
                ? 1
                : (int)Math.Ceiling((double)totalItems / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var lopHocs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gắn lại chương trình đào tạo cho các lớp trong trang (giữ nguyên xử lý cũ).
            var maChuongTrinhTrongTrang = lopHocs
                .Select(x => x.MaChuongTrinhDaoTao)
                .Distinct()
                .ToList();

            var chuongTrinhDaoTaos =
                await _context.ChuongTrinhDaoTaos
                    .Where(x =>
                        maChuongTrinhTrongTrang.Contains(
                            x.MaChuongTrinhDaoTao))
                    .ToDictionaryAsync(
                        x => x.MaChuongTrinhDaoTao,
                        x => x);

            foreach (var lopHoc in lopHocs)
            {
                if (chuongTrinhDaoTaos.TryGetValue(
                    lopHoc.MaChuongTrinhDaoTao,
                    out var chuongTrinh))
                {
                    lopHoc.ChuongTrinhDaoTao = chuongTrinh;
                }
            }

            return (lopHocs, totalItems);
        }

        public Task<List<ChuongTrinhDaoTao>> LayChuongTrinhDaoTaoAsync(bool chiDangHoatDong)
        {
            IQueryable<ChuongTrinhDaoTao> query = _context.ChuongTrinhDaoTaos;
            if (chiDangHoatDong)
                query = query.Where(x => x.TrangThai);

            return query
                .OrderBy(x => x.TenChuongTrinhDaoTao)
                .ToListAsync();
        }

        public Task<List<string>> LayDanhSachTrinhDoDauVaoAsync() =>
            _context.LopHocs
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.TrinhDoDauVao))
                .Select(x => x.TrinhDoDauVao)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

        public Task<LopHoc?> LayChiTietAsync(int maLop) =>
            _context.LopHocs
                .Include(x => x.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(x => x.MaLop == maLop);

        public Task<LopHoc?> TimTheoMaAsync(int maLop) =>
            _context.LopHocs
                .FirstOrDefaultAsync(x => x.MaLop == maLop);

        public Task<bool> TonTaiAsync(int maLop) =>
            _context.LopHocs
                .AsNoTracking()
                .AnyAsync(x => x.MaLop == maLop);

        public Task<bool> ChuongTrinhDangHoatDongAsync(int maChuongTrinhDaoTao) =>
            _context.ChuongTrinhDaoTaos
                .AnyAsync(x =>
                    x.MaChuongTrinhDaoTao == maChuongTrinhDaoTao &&
                    x.TrangThai);

        public Task<int> DemSoHocVienDaXepLopAsync(int maLop) =>
            _context.HoSoDangKyHocs
                .CountAsync(x =>
                    x.MaLop == maLop &&
                    x.TrangThai == TrangThaiHoSo.DuocXepLop);

        public Task<bool> CoHoSoDangKyAsync(int maLop) =>
            _context.HoSoDangKyHocs
                .AnyAsync(x => x.MaLop == maLop);

        public void Them(LopHoc lopHoc) => _context.LopHocs.Add(lopHoc);

        public async Task<bool> CapNhatAsync(LopHoc lopHoc)
        {
            try
            {
                _context.Update(lopHoc);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists =
                    await _context.LopHocs
                        .AnyAsync(x => x.MaLop == lopHoc.MaLop);

                if (!exists)
                    return false;

                throw;
            }
        }

        public void Xoa(LopHoc lopHoc) => _context.LopHocs.Remove(lopHoc);

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
