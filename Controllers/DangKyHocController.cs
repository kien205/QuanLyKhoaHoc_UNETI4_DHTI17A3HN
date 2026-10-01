// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: Xem lớp, nộp, theo dõi và hủy hồ sơ đăng ký học.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.HocVien)]
    public class DangKyHocController : HocVienModuleController
    {
        private static readonly string[] TrangThaiHoSoDangXuLy =
        {
            TrangThaiHoSo.ChoDuyet,
            TrangThaiHoSo.DuDieuKien,
            TrangThaiHoSo.ChoKiemTraDauVao,
            TrangThaiHoSo.DaKiemTraDauVao
        };

        public DangKyHocController(AppDbContext db) : base(db)
        {
        }

        public async Task<IActionResult> LopDangMo()
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            var homNay = DateTime.Today;
            var lopHocs = await Db.LopHocs
                .AsNoTracking()
                .Include(l => l.ChuongTrinhDaoTao)
                .Where(l => l.TrangThai == TrangThaiLop.DangMoDangKy)
                .OrderBy(l => l.HanDangKy)
                .ThenBy(l => l.TenLop)
                .ToListAsync();

            var maLops = lopHocs.Select(l => l.MaLop).ToArray();
            var lopDaDangKy = await Db.HoSoDangKyHocs
                .AsNoTracking()
                .Where(h => h.MaHocVien == hocVien.MaHocVien
                    && maLops.Contains(h.MaLop)
                    && TrangThaiHoSoDangXuLy.Contains(h.TrangThai))
                .Select(h => h.MaLop)
                .ToListAsync();

            var ds = lopHocs.Select(l => new LopHocDangKyViewModel
            {
                MaLop = l.MaLop,
                TenLop = l.TenLop,
                TenChuongTrinhDaoTao = l.ChuongTrinhDaoTao?.TenChuongTrinhDaoTao ?? string.Empty,
                TrinhDoDauVao = l.TrinhDoDauVao,
                DiemDauVao = l.DiemDauVao,
                NgayBatDauDangKy = l.NgayBatDauDangKy,
                HanDangKy = l.HanDangKy,
                DangTrongHanDangKy = l.NgayBatDauDangKy.Date <= homNay && l.HanDangKy.Date >= homNay,
                DaCoHoSoDangXuLy = lopDaDangKy.Contains(l.MaLop)
            }).ToList();

            return View(ds);
        }

        [HttpGet]
        public async Task<IActionResult> NopHoSo(int maLop)
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            if (!hocVien.TrangThai)
            {
                TempData["Loi"] = "Hồ sơ học viên đang bị khóa, bạn chưa thể nộp đăng ký.";
                return RedirectToAction(nameof(LopDangMo));
            }

            if (!HoSoCaNhanDayDu(hocVien))
            {
                TempData["Loi"] = "Vui lòng cập nhật họ tên, ngày sinh, giới tính và ít nhất một thông tin liên hệ trước khi đăng ký.";
                return RedirectToAction(nameof(LopDangMo));
            }

            var lopHopLe = await LopConNhanHoSoAsync(maLop);
            if (!lopHopLe)
            {
                TempData["Loi"] = "Lớp không tồn tại, chưa mở đăng ký hoặc đã ngoài thời gian nhận hồ sơ.";
                return RedirectToAction(nameof(LopDangMo));
            }

            if (await CoHoSoDangXuLyAsync(hocVien.MaHocVien, maLop))
            {
                TempData["Loi"] = "Bạn đã có hồ sơ đang xử lý cho lớp này.";
                return RedirectToAction(nameof(LopDangMo));
            }

            var lop = await Db.LopHocs
                .AsNoTracking()
                .Include(l => l.ChuongTrinhDaoTao)
                .SingleAsync(l => l.MaLop == maLop);
            ViewBag.TenLop = lop.TenLop;
            ViewBag.TenChuongTrinh = lop.ChuongTrinhDaoTao?.TenChuongTrinhDaoTao;
            return View(new NopHoSoDangKyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NopHoSo(int maLop, NopHoSoDangKyViewModel model)
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            if (!hocVien.TrangThai)
            {
                TempData["Loi"] = "Hồ sơ học viên đang bị khóa, bạn chưa thể nộp đăng ký.";
                return RedirectToAction(nameof(LopDangMo));
            }

            if (!ModelState.IsValid)
            {
                await NapThongTinLopAsync(maLop);
                return View(model);
            }

            if (!HoSoCaNhanDayDu(hocVien))
            {
                TempData["Loi"] = "Vui lòng cập nhật hồ sơ cá nhân trước khi đăng ký.";
                return RedirectToAction(nameof(LopDangMo));
            }

            if (!await LopConNhanHoSoAsync(maLop))
            {
                TempData["Loi"] = "Lớp không tồn tại, chưa mở đăng ký hoặc đã ngoài thời gian nhận hồ sơ.";
                return RedirectToAction(nameof(LopDangMo));
            }

            if (await CoHoSoDangXuLyAsync(hocVien.MaHocVien, maLop))
            {
                TempData["Loi"] = "Bạn đã có hồ sơ đang xử lý cho lớp này.";
                return RedirectToAction(nameof(HoSoCuaToi));
            }

            Db.HoSoDangKyHocs.Add(new HoSoDangKyHoc
            {
                MaHocVien = hocVien.MaHocVien,
                MaLop = maLop,
                NgayNop = DateTime.Now,
                LyDoDangKy = model.LyDoDangKy?.Trim(),
                TrangThai = TrangThaiHoSo.ChoDuyet
            });

            await Db.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã nộp hồ sơ đăng ký học.";
            return RedirectToAction(nameof(HoSoCuaToi));
        }

        public async Task<IActionResult> HoSoCuaToi()
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            var hoSos = await Db.HoSoDangKyHocs
                .AsNoTracking()
                .Where(h => h.MaHocVien == hocVien.MaHocVien)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l!.ChuongTrinhDaoTao)
                .Include(h => h.LichKiemTraDauVaos)
                .Include(h => h.KetQuaXepLop)
                .OrderByDescending(h => h.NgayNop)
                .ToListAsync();

            var ds = hoSos.Select(h => new HoSoHocVienViewModel
            {
                MaHoSo = h.MaHoSo,
                TenLop = h.LopHoc?.TenLop ?? string.Empty,
                TenChuongTrinhDaoTao = h.LopHoc?.ChuongTrinhDaoTao?.TenChuongTrinhDaoTao ?? string.Empty,
                NgayNop = h.NgayNop,
                TrangThai = h.TrangThai,
                KetQuaXepLop = h.KetQuaXepLop?.KetQua,
                CoTheHuy = CoTheHuy(h),
                LichKiemTra = h.LichKiemTraDauVaos
                    .OrderBy(l => l.ThoiGianBatDau)
                    .Select(l => new LichHocVienViewModel
                    {
                        ThoiGianBatDau = l.ThoiGianBatDau,
                        ThoiGianKetThuc = l.ThoiGianKetThuc,
                        HinhThucKiemTra = l.HinhThucKiemTra,
                        DiaDiemHoacLienKet = l.DiaDiemHoacLienKet,
                        GiaoVienKiemTra = l.GiaoVienKiemTra,
                        TrangThai = l.TrangThai
                    }).ToList()
            }).ToList();

            return View(ds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyHoSo(int maHoSo)
        {
            var hocVien = await LayHocVienHienTaiAsync();
            if (hocVien == null)
                return BaoThieuHoSoHocVien();

            var hoSo = await Db.HoSoDangKyHocs
                .Include(h => h.LichKiemTraDauVaos)
                .Include(h => h.KetQuaXepLop)
                .SingleOrDefaultAsync(h => h.MaHoSo == maHoSo && h.MaHocVien == hocVien.MaHocVien);

            if (hoSo == null)
                return NotFound();

            if (!CoTheHuy(hoSo))
            {
                TempData["Loi"] = "Hồ sơ này đã có lịch kiểm tra đang hiệu lực, kết quả hoặc không còn ở trạng thái được phép hủy.";
                return RedirectToAction(nameof(HoSoCuaToi));
            }

            hoSo.TrangThai = TrangThaiHoSo.DaHuy;
            await Db.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã hủy hồ sơ đăng ký học.";
            return RedirectToAction(nameof(HoSoCuaToi));
        }

        private Task<bool> LopConNhanHoSoAsync(int maLop)
        {
            var homNay = DateTime.Today;
            return Db.LopHocs.AnyAsync(l =>
                l.MaLop == maLop
                && l.TrangThai == TrangThaiLop.DangMoDangKy
                && l.NgayBatDauDangKy < homNay.AddDays(1)
                && l.HanDangKy >= homNay);
        }

        private Task<bool> CoHoSoDangXuLyAsync(int maHocVien, int maLop) =>
            Db.HoSoDangKyHocs.AnyAsync(h =>
                h.MaHocVien == maHocVien
                && h.MaLop == maLop
                && TrangThaiHoSoDangXuLy.Contains(h.TrangThai));

        private static bool HoSoCaNhanDayDu(HocVien hocVien) =>
            !string.IsNullOrWhiteSpace(hocVien.HoTen)
            && hocVien.NgaySinh != default
            && hocVien.NgaySinh.Date <= DateTime.Today
            && !string.IsNullOrWhiteSpace(hocVien.GioiTinh)
            && (!string.IsNullOrWhiteSpace(hocVien.SoDienThoai)
                || !string.IsNullOrWhiteSpace(hocVien.Email));

        private static bool CoTheHuy(HoSoDangKyHoc hoSo) =>
            hoSo.KetQuaXepLop == null
            && !hoSo.LichKiemTraDauVaos.Any(l => l.TrangThai != TrangThaiLich.DaHuy)
            && (hoSo.TrangThai == TrangThaiHoSo.ChoDuyet
                || hoSo.TrangThai == TrangThaiHoSo.DuDieuKien);

        private async Task NapThongTinLopAsync(int maLop)
        {
            var lop = await Db.LopHocs
                .AsNoTracking()
                .Include(l => l.ChuongTrinhDaoTao)
                .SingleOrDefaultAsync(l => l.MaLop == maLop);
            ViewBag.TenLop = lop?.TenLop ?? string.Empty;
            ViewBag.TenChuongTrinh = lop?.ChuongTrinhDaoTao?.TenChuongTrinhDaoTao ?? string.Empty;
        }
    }
}
