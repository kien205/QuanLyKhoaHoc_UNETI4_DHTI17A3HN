// Repository cho HocVienController (và lớp cha HocVienModuleController).
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories
{
    public class HocVienRepository : IHocVienRepository
    {
        private readonly AppDbContext _context;

        public HocVienRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<HocVien?> LayTheoMaTaiKhoanAsync(int maTaiKhoan, bool theoDoiThayDoi = false)
        {
            IQueryable<HocVien> hocViens = _context.HocViens;
            if (!theoDoiThayDoi)
                hocViens = hocViens.AsNoTracking();

            return hocViens.SingleOrDefaultAsync(h => h.MaTaiKhoan == maTaiKhoan);
        }

        public Task LuuThayDoiAsync() => _context.SaveChangesAsync();
    }
}
