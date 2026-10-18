// Họ và tên: Tạ Thị Bích Loan 
// Mã sinh viên: 23103100147
// Nội dung thực hiện: Module 4 - Tiếp nhận hồ sơ, Xét duyệt, Quản lý luồng trạng thái, Kiểm soát sĩ số tối đa
// Sửa lỗi xác thực kết quả xét duyệt: Nguyễn Văn Giáp, 22103100110.
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
  

    [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
    public class HoSoXetDuyetController : Controller
    {
        private readonly AppDbContext _context;

        public HoSoXetDuyetController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string tuKhoa,
            int? maChuongTrinh,
            string trangThai,
            DateTime? tuNgay,
            DateTime? denNgay,
            string sapXep,
            int trang = 1)
        {
            int pageSize = 10;
            if (trang < 1) trang = 1;

           
            var query = _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string tk = tuKhoa.Trim().ToLower();
                query = query.Where(h => (h.HocVien != null && h.HocVien.HoTen.ToLower().Contains(tk)) ||
                                         (h.HocVien != null && h.HocVien.Email != null &&
                                          h.HocVien.Email.ToLower().Contains(tk)) ||
                                         (h.LopHoc != null && h.LopHoc.TenLop.ToLower().Contains(tk)));
            }

           
            if (maChuongTrinh.HasValue)
            {
                query = query.Where(h => h.LopHoc != null && h.LopHoc.MaChuongTrinhDaoTao == maChuongTrinh.Value);
            }

        
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(h => h.TrangThai == trangThai);
            }

            
            if (tuNgay.HasValue)
            {
                query = query.Where(h => h.NgayNop >= tuNgay.Value.Date);
            }
            if (denNgay.HasValue)
            {
                DateTime denNgayCuoiNgay = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(h => h.NgayNop <= denNgayCuoiNgay);
            }

         
            query = sapXep switch
            {
                "ngay_tang" => query.OrderBy(h => h.NgayNop),
                "ten_az" => query.OrderBy(h => h.HocVien != null ? h.HocVien.HoTen : string.Empty),
                "ten_za" => query.OrderByDescending(h => h.HocVien != null ? h.HocVien.HoTen : string.Empty),
                _ => query.OrderByDescending(h => h.NgayNop)
            };

            int tongBanGhi = await query.CountAsync();
            int tongSoTrang = (int)Math.Ceiling(tongBanGhi / (double)pageSize);
            if (tongSoTrang > 0 && trang > tongSoTrang) trang = tongSoTrang;

            var danhSach = await query
                .Skip((trang - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            var chuongTrinhs = await _context.ChuongTrinhDaoTaos.OrderBy(c => c.TenChuongTrinhDaoTao).ToListAsync();
            ViewBag.ChuongTrinhList = new SelectList(chuongTrinhs, "MaChuongTrinhDaoTao", "TenChuongTrinhDaoTao", maChuongTrinh);

            var viewModel = new DanhSachHoSoViewModel
            {
                DanhSachHoSo = danhSach,
                TuKhoa = tuKhoa,
                MaChuongTrinhDaoTao = maChuongTrinh,
                TrangThai = trangThai,
                TuNgay = tuNgay,
                DenNgay = denNgay,
                SapXep = sapXep,
                TrangHienTai = trang,
                TongSoTrang = tongSoTrang,
                TongSoBanGhi = tongBanGhi
            };

            return View(viewModel);
        }

      
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hoSo = await _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(h => h.MaHoSo == id);

            if (hoSo == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ yêu cầu.";
                return RedirectToAction(nameof(Index));
            }

            // Tính sĩ số đã xếp lớp
            int daXepLop = 0;
            int siSoToiDa = hoSo.LopHoc?.SiSoToiDa ?? 0;
            if (hoSo.MaLop > 0)
            {
                daXepLop = await _context.HoSoDangKyHocs
                    .CountAsync(h => h.MaLop == hoSo.MaLop && h.TrangThai == TrangThaiHoSo.DuocXepLop);
            }

            ViewBag.SoLuongDaXepLop = daXepLop;
            ViewBag.SiSoToiDa = siSoToiDa;
            ViewBag.DaDaySiSo = (siSoToiDa > 0 && daXepLop >= siSoToiDa);

            return View(hoSo);
        }

     
        [HttpGet]
        public async Task<IActionResult> XetDuyet(int id)
        {
            var hoSo = await _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(h => h.MaHoSo == id);

            if (hoSo == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ cần xét duyệt.";
                return RedirectToAction(nameof(Index));
            }

            if (hoSo.TrangThai != TrangThaiHoSo.ChoDuyet)
            {
                TempData["ErrorMessage"] = "Chỉ hồ sơ ở trạng thái 'Chờ duyệt' mới được phép xét duyệt!";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            if (hoSo.HocVien == null || hoSo.LopHoc == null)
            {
                TempData["ErrorMessage"] = "Hồ sơ thiếu thông tin học viên hoặc lớp học nên không thể xét duyệt.";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            if (!hoSo.HocVien.TrangThai)
            {
                TempData["ErrorMessage"] = "Học viên đang bị khóa nên hồ sơ không thể được xét duyệt.";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            int daXepLop = 0;
            int siSoToiDa = hoSo.LopHoc?.SiSoToiDa ?? 0;
            if (hoSo.MaLop > 0)
            {
                daXepLop = await _context.HoSoDangKyHocs
                    .CountAsync(h => h.MaLop == hoSo.MaLop && h.TrangThai == TrangThaiHoSo.DuocXepLop);
            }

            var viewModel = new XetDuyetHoSoViewModel
            {
                MaHoSo = hoSo.MaHoSo,
                HoSo = hoSo,
                SiSoToiDa = siSoToiDa,
                SoLuongDaXepLop = daXepLop,
                KetQuaDuyet = TrangThaiHoSo.DuDieuKien
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XetDuyet(XetDuyetHoSoViewModel model)
        {
            var hoSo = await _context.HoSoDangKyHocs
                .Include(h => h.HocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(h => h.MaHoSo == model.MaHoSo);

            if (hoSo == null)
            {
                TempData["ErrorMessage"] = "Hồ sơ không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            if (hoSo.TrangThai != TrangThaiHoSo.ChoDuyet)
            {
                TempData["ErrorMessage"] = "Hồ sơ này đã được xử lý trước đó rồi!";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            if (hoSo.HocVien == null || hoSo.LopHoc == null)
            {
                TempData["ErrorMessage"] = "Hồ sơ thiếu thông tin học viên hoặc lớp học nên không thể xét duyệt.";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            if (!hoSo.HocVien.TrangThai)
            {
                TempData["ErrorMessage"] = "Học viên đang bị khóa nên hồ sơ không thể được xét duyệt.";
                return RedirectToAction(nameof(ChiTiet), new { id = hoSo.MaHoSo });
            }

            if (model.KetQuaDuyet != TrangThaiHoSo.DuDieuKien &&
                model.KetQuaDuyet != TrangThaiHoSo.KhongDuDieuKien)
            {
                ModelState.AddModelError(
                    nameof(model.KetQuaDuyet),
                    "Vui lòng chọn một kết quả xét duyệt hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                model.HoSo = hoSo;
                model.SiSoToiDa = hoSo.LopHoc?.SiSoToiDa ?? 0;
                int daXepLop = 0;
                if (hoSo.MaLop > 0)
                {
                    daXepLop = await _context.HoSoDangKyHocs
                        .CountAsync(h => h.MaLop == hoSo.MaLop && h.TrangThai == TrangThaiHoSo.DuocXepLop);
                }
                model.SoLuongDaXepLop = daXepLop;
                return View(model);
            }

          
            hoSo.TrangThai = model.KetQuaDuyet;
            hoSo.NgayXuLy = DateTime.Now;
            hoSo.NhanXetXetDuyet = model.NhanXetXetDuyet;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã xét duyệt thành công hồ sơ #{hoSo.MaHoSo} ({model.KetQuaDuyet})!";
            return RedirectToAction(nameof(Index));
        }
    }
}