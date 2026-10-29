// Họ và tên: Tạ Thị Bích Loan
// Mã sinh viên: 23103100147
// Nội dung thực hiện: Module 4 - Tiếp nhận hồ sơ, Xét duyệt, Quản lý luồng trạng thái, Kiểm soát sĩ số tối đa
// Sửa lỗi xác thực kết quả xét duyệt: Nguyễn Văn Giáp, 22103100110.
// Truy vấn CSDL được tách sang IHoSoXetDuyetRepository.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
    public class HoSoXetDuyetController : Controller
    {
        private readonly IHoSoXetDuyetRepository _repo;

        public HoSoXetDuyetController(IHoSoXetDuyetRepository repo)
        {
            _repo = repo;
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

            var (danhSach, tongBanGhi) = await _repo.TimKiemAsync(
                tuKhoa, maChuongTrinh, trangThai, tuNgay, denNgay, sapXep, trang, pageSize);

            int tongSoTrang = (int)Math.Ceiling(tongBanGhi / (double)pageSize);
            if (tongSoTrang > 0 && trang > tongSoTrang) trang = tongSoTrang;

            var chuongTrinhs = await _repo.LayDanhSachChuongTrinhAsync();
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
            var hoSo = await _repo.LayChiTietHoSoAsync(id);

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
                daXepLop = await _repo.DemSoDaXepLopAsync(hoSo.MaLop);
            }

            ViewBag.SoLuongDaXepLop = daXepLop;
            ViewBag.SiSoToiDa = siSoToiDa;
            ViewBag.DaDaySiSo = (siSoToiDa > 0 && daXepLop >= siSoToiDa);

            return View(hoSo);
        }


        [HttpGet]
        public async Task<IActionResult> XetDuyet(int id)
        {
            var hoSo = await _repo.LayChiTietHoSoAsync(id);

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
                daXepLop = await _repo.DemSoDaXepLopAsync(hoSo.MaLop);
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
            var hoSo = await _repo.LayChiTietHoSoAsync(model.MaHoSo);

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
                    daXepLop = await _repo.DemSoDaXepLopAsync(hoSo.MaLop);
                }
                model.SoLuongDaXepLop = daXepLop;
                return View(model);
            }


            hoSo.TrangThai = model.KetQuaDuyet;
            hoSo.NgayXuLy = DateTime.Now;
            hoSo.NhanXetXetDuyet = model.NhanXetXetDuyet;

            await _repo.LuuThayDoiAsync();

            TempData["SuccessMessage"] = $"Đã xét duyệt thành công hồ sơ #{hoSo.MaHoSo} ({model.KetQuaDuyet})!";
            return RedirectToAction(nameof(Index));
        }
    }
}
