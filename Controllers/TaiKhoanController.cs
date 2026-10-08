// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using Microsoft.AspNetCore.Mvc;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly AppDbContext _db;
        public TaiKhoanController(AppDbContext db) => _db = db;

        [HttpGet]
        public IActionResult DangNhap(string? returnUrl) 
        { 
            ViewBag.ReturnUrl = returnUrl; return View(); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DangNhap(DangNhapViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var tk = _db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == model.TenDangNhap);
            if (tk == null) 
            { 
                ModelState.AddModelError("", "Tài khoản không tồn tại."); return View(model); 
            }
            if (tk.MatKhau != model.MatKhau) 
            { 
                ModelState.AddModelError("", "Sai mật khẩu."); return View(model); 
            }
            if (!tk.TrangThai) 
            { 
                ModelState.AddModelError("", "Tài khoản đã bị khóa."); return View(model); 
            }

            HttpContext.Session.SetInt32(SessionKeys.MaTaiKhoan, tk.MaTaiKhoan);
            HttpContext.Session.SetString(SessionKeys.HoTen, tk.HoTen);
            HttpContext.Session.SetString(SessionKeys.VaiTro, tk.VaiTro);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
        }

        public IActionResult TuChoiTruyCap() => View();
    }
}
