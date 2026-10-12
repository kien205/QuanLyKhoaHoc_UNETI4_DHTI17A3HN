// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: Helper dùng chung cho các chức năng học viên.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    public abstract class HocVienModuleController : Controller
    {
        protected readonly AppDbContext Db;

        protected HocVienModuleController(AppDbContext db)
        {
            Db = db;
        }

        protected Task<HocVien?> LayHocVienHienTaiAsync(bool theoDoiThayDoi = false)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32(SessionKeys.MaTaiKhoan);
            if (maTaiKhoan == null)
                return Task.FromResult<HocVien?>(null);

            IQueryable<HocVien> hocViens = Db.HocViens;
            if (!theoDoiThayDoi)
                hocViens = hocViens.AsNoTracking();

            return hocViens.SingleOrDefaultAsync(h => h.MaTaiKhoan == maTaiKhoan.Value);
        }

        protected IActionResult BaoThieuHoSoHocVien()
        {
            TempData["Loi"] = "Tài khoản hiện tại chưa được liên kết với hồ sơ học viên.";
            return RedirectToAction("Index", "Home");
        }
    }
}
