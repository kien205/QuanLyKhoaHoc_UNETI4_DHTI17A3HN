// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: Xem và cập nhật hồ sơ cá nhân học viên.

using Microsoft.AspNetCore.Mvc;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.HocVien)]
    public class HocVienController : HocVienModuleController
    {
        public HocVienController(AppDbContext db) : base(db)
        {
        }

        public async Task<IActionResult> Index()
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            return View(ChuyenDoi(hocVien));
        }

        [HttpGet]
        public async Task<IActionResult> CapNhatHoSo()
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            return View(ChuyenDoi(hocVien));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatHoSo(HoSoCaNhanViewModel model)
        {
            var hocVien = await LayHocVienHienTaiAsync(theoDoiThayDoi: true);
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            if (model.NgaySinh == default || model.NgaySinh.Date > DateTime.Today)
                ModelState.AddModelError(nameof(model.NgaySinh), "Ngày sinh phải hợp lệ và không được ở tương lai.");

            if (model.GioiTinh is not ("Nam" or "Nữ" or "Khác"))
                ModelState.AddModelError(nameof(model.GioiTinh), "Vui lòng chọn giới tính hợp lệ.");

            if (!ModelState.IsValid)
                return View(model);

            hocVien.HoTen = model.HoTen.Trim();
            hocVien.NgaySinh = model.NgaySinh.Date;
            hocVien.GioiTinh = model.GioiTinh;
            hocVien.SoDienThoai = model.SoDienThoai?.Trim();
            hocVien.Email = model.Email?.Trim();
            hocVien.DiaChi = model.DiaChi?.Trim();
            hocVien.TrinhDoHienTai = model.TrinhDoHienTai?.Trim();
            hocVien.NgheNghiep = model.NgheNghiep?.Trim();
            hocVien.SoThangDaHoc = model.SoThangDaHoc;
            hocVien.MucTieuHoc = model.MucTieuHoc?.Trim();

            await Db.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã cập nhật hồ sơ cá nhân.";
            return RedirectToAction(nameof(Index));
        }

        private static HoSoCaNhanViewModel ChuyenDoi(HocVien hocVien) => new()
        {
            HoTen = hocVien.HoTen,
            NgaySinh = hocVien.NgaySinh,
            GioiTinh = hocVien.GioiTinh,
            SoDienThoai = hocVien.SoDienThoai,
            Email = hocVien.Email,
            DiaChi = hocVien.DiaChi,
            TrinhDoHienTai = hocVien.TrinhDoHienTai,
            NgheNghiep = hocVien.NgheNghiep,
            SoThangDaHoc = hocVien.SoThangDaHoc,
            MucTieuHoc = hocVien.MucTieuHoc
        };
    }
}
