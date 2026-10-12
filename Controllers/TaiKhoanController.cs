// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Đăng nhập, đăng xuất, Session và quản lý tài khoản (Admin).
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly AppDbContext _db;
        public TaiKhoanController(AppDbContext db) => _db = db;

        private static readonly string[] DanhSachVaiTro =
            { VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.HocVien };

        private void NapVaiTro() => ViewBag.DanhSachVaiTro = new SelectList(DanhSachVaiTro);

        [HttpGet]
        public IActionResult DangNhap(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DangNhap(DangNhapViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var tk = _db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == model.TenDangNhap);
            if (tk == null) { ModelState.AddModelError("", "Tài khoản không tồn tại."); return View(model); }
            if (tk.MatKhau != model.MatKhau) { ModelState.AddModelError("", "Sai mật khẩu."); return View(model); }
            if (!tk.TrangThai) { ModelState.AddModelError("", "Tài khoản đã bị khóa."); return View(model); }

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

    
        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Index(string? tuKhoa, string? vaiTro)
        {
            var q = _db.TaiKhoans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                q = q.Where(t => t.TenDangNhap.Contains(tuKhoa)
                              || t.HoTen.Contains(tuKhoa)
                              || t.Email.Contains(tuKhoa));
            }
            if (!string.IsNullOrWhiteSpace(vaiTro))
                q = q.Where(t => t.VaiTro == vaiTro);

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.VaiTro = vaiTro;
            return View(await q.OrderBy(t => t.TenDangNhap).ToListAsync());
        }

        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public IActionResult Create()
        {
            NapVaiTro();
            return View(new TaiKhoanCreateViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Create(TaiKhoanCreateViewModel model)
        {
            model.TenDangNhap = model.TenDangNhap?.Trim() ?? string.Empty;

            if (!DanhSachVaiTro.Contains(model.VaiTro))
                ModelState.AddModelError(nameof(model.VaiTro), "Vai trò không hợp lệ.");

            if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
                ModelState.AddModelError(nameof(model.TenDangNhap), "Tên đăng nhập đã tồn tại.");

            if (!ModelState.IsValid)
            {
                NapVaiTro();
                return View(model);
            }

            _db.TaiKhoans.Add(new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = model.MatKhau,
                HoTen = model.HoTen.Trim(),
                Email = model.Email.Trim(),
                VaiTro = model.VaiTro,
                TrangThai = true
            });
            await _db.SaveChangesAsync();

            TempData["ThanhCong"] = "Đã thêm tài khoản.";
            return RedirectToAction(nameof(Index));
        }

        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var tk = await _db.TaiKhoans.FindAsync(id);
            if (tk == null) return NotFound();

            NapVaiTro();
            return View(new TaiKhoanEditViewModel
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                TenDangNhap = tk.TenDangNhap,
                HoTen = tk.HoTen,
                Email = tk.Email,
                VaiTro = tk.VaiTro
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Edit(int id, TaiKhoanEditViewModel model)
        {
            if (id != model.MaTaiKhoan) return NotFound();
            var tk = await _db.TaiKhoans.FindAsync(id);
            if (tk == null) return NotFound();

            var maHienTai = HttpContext.Session.GetInt32(SessionKeys.MaTaiKhoan);

            if (!DanhSachVaiTro.Contains(model.VaiTro))
                ModelState.AddModelError(nameof(model.VaiTro), "Vai trò không hợp lệ.");

        
            if (tk.MaTaiKhoan == maHienTai && model.VaiTro != tk.VaiTro)
                ModelState.AddModelError(nameof(model.VaiTro), "Không thể tự đổi vai trò của chính mình.");

            if (!ModelState.IsValid)
            {
                model.TenDangNhap = tk.TenDangNhap;
                NapVaiTro();
                return View(model);
            }

            tk.HoTen = model.HoTen.Trim();
            tk.Email = model.Email.Trim();
            tk.VaiTro = model.VaiTro;
            if (!string.IsNullOrWhiteSpace(model.MatKhauMoi))
                tk.MatKhau = model.MatKhauMoi;

            await _db.SaveChangesAsync();

            if (tk.MaTaiKhoan == maHienTai)
                HttpContext.Session.SetString(SessionKeys.HoTen, tk.HoTen);   

            TempData["ThanhCong"] = "Đã cập nhật tài khoản.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var tk = await _db.TaiKhoans.FindAsync(id);
            if (tk == null) return NotFound();

            if (tk.MaTaiKhoan == HttpContext.Session.GetInt32(SessionKeys.MaTaiKhoan))
            {
                TempData["Loi"] = "Không thể tự khóa tài khoản của chính mình.";
                return RedirectToAction(nameof(Index));
            }

            tk.TrangThai = !tk.TrangThai;
            await _db.SaveChangesAsync();

            TempData["ThanhCong"] = tk.TrangThai ? "Đã mở khóa tài khoản." : "Đã khóa tài khoản.";
            return RedirectToAction(nameof(Index));
        }
    }
}