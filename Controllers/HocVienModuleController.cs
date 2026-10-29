// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: Helper dùng chung cho các chức năng học viên.
// Truy vấn CSDL được tách sang IHocVienRepository.

using Microsoft.AspNetCore.Mvc;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    public abstract class HocVienModuleController : Controller
    {
        protected readonly IHocVienRepository HocVienRepo;

        protected HocVienModuleController(IHocVienRepository hocVienRepo)
        {
            HocVienRepo = hocVienRepo;
        }

        protected Task<HocVien?> LayHocVienHienTaiAsync(bool theoDoiThayDoi = false)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32(SessionKeys.MaTaiKhoan);
            if (maTaiKhoan == null)
                return Task.FromResult<HocVien?>(null);

            return HocVienRepo.LayTheoMaTaiKhoanAsync(maTaiKhoan.Value, theoDoiThayDoi);
        }

        protected IActionResult BaoThieuHoSoHocVien()
        {
            TempData["Loi"] = "Tài khoản hiện tại chưa được liên kết với hồ sơ học viên.";
            return RedirectToAction("Index", "Home");
        }
    }
}
