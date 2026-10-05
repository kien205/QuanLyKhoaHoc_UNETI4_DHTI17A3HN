// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Lịch kiểm tra đầu vào: lập lịch, kiểm tra trùng lịch,
// hủy lịch, hoàn thành lịch.

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
    [PhanQuyen(VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao)]
    public class LichKiemTraDauVaoController : Controller
    {
        private readonly AppDbContext _context;

        public LichKiemTraDauVaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: LichKiemTraDauVao
        public async Task<IActionResult> Index()
        {
            var ds = await _context.LichKiemTraDauVaos
                .Include(l => l.HoSoDangKyHoc!).ThenInclude(h => h.HocVien)
                .Include(l => l.HoSoDangKyHoc!).ThenInclude(h => h.LopHoc)
                .OrderByDescending(l => l.ThoiGianBatDau)
                .ToListAsync();
            return View(ds);
        }

        // GET: LichKiemTraDauVao/Create
        public async Task<IActionResult> Create(int? maHoSo)
        {
            var vm = new LapLichViewModel { MaHoSo = maHoSo };
            await NapDanhSachHoSo(vm);
            return View(vm);
        }

        // POST: LichKiemTraDauVao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LapLichViewModel vm)
        {
            vm.GiaoVienKiemTra = vm.GiaoVienKiemTra?.Trim() ?? string.Empty;

            if (ModelState.IsValid)
            {
                var batDau = vm.ThoiGianBatDau!.Value;
                var ketThuc = vm.ThoiGianKetThuc!.Value;

                var hoSo = await _context.HoSoDangKyHocs
                    .FirstOrDefaultAsync(h => h.MaHoSo == vm.MaHoSo);
                if (hoSo == null)
                {
                    ModelState.AddModelError(nameof(vm.MaHoSo), "Hồ sơ không tồn tại.");
                }
                else if (hoSo.TrangThai != TrangThaiHoSo.DuDieuKien)
                {
                    ModelState.AddModelError(nameof(vm.MaHoSo),
                        "Chỉ hồ sơ ở trạng thái \"Đủ điều kiện\" mới được lập lịch kiểm tra đầu vào.");
                }

      
                if (ketThuc <= batDau)
                {
                    ModelState.AddModelError(nameof(vm.ThoiGianKetThuc),
                        "Thời gian kết thúc phải sau thời gian bắt đầu.");
                }

           
                if (batDau < DateTime.Now)
                {
                    ModelState.AddModelError(nameof(vm.ThoiGianBatDau),
                        "Thời gian kiểm tra không được nằm trong quá khứ.");
                }

                if (hoSo != null && ketThuc > batDau)
                {
                    bool daCoLich = await _context.LichKiemTraDauVaos.AnyAsync(l =>
                        l.MaHoSo == hoSo.MaHoSo &&
                        l.TrangThai != TrangThaiLich.DaHuy);
                    if (daCoLich)
                    {
                        ModelState.AddModelError(nameof(vm.MaHoSo),
                            "Hồ sơ này đã có lịch kiểm tra đầu vào còn hiệu lực.");
                    }

                    bool trungHocVien = await _context.LichKiemTraDauVaos.AnyAsync(l =>
                        l.TrangThai != TrangThaiLich.DaHuy &&
                        l.HoSoDangKyHoc!.MaHocVien == hoSo.MaHocVien &&
                        l.ThoiGianBatDau < ketThuc &&
                        l.ThoiGianKetThuc > batDau);
                    if (trungHocVien)
                    {
                        ModelState.AddModelError(nameof(vm.ThoiGianBatDau),
                            "Học viên đã có lịch kiểm tra đầu vào khác trùng khoảng thời gian này.");
                    }

                    bool trungGiaoVien = await _context.LichKiemTraDauVaos.AnyAsync(l =>
                        l.TrangThai != TrangThaiLich.DaHuy &&
                        l.GiaoVienKiemTra == vm.GiaoVienKiemTra &&
                        l.ThoiGianBatDau < ketThuc &&
                        l.ThoiGianKetThuc > batDau);
                    if (trungGiaoVien)
                    {
                        ModelState.AddModelError(nameof(vm.GiaoVienKiemTra),
                            "Giáo viên kiểm tra đã có lịch khác trùng khoảng thời gian này.");
                    }
                }

                if (ModelState.IsValid)
                {
                    var lich = new LichKiemTraDauVao
                    {
                        MaHoSo = hoSo!.MaHoSo,
                        ThoiGianBatDau = batDau,
                        ThoiGianKetThuc = ketThuc,
                        HinhThucKiemTra = vm.HinhThucKiemTra.Trim(),
                        DiaDiemHoacLienKet = vm.DiaDiemHoacLienKet?.Trim(),
                        GiaoVienKiemTra = vm.GiaoVienKiemTra,
                        GhiChu = vm.GhiChu?.Trim(),
                        TrangThai = TrangThaiLich.DaLenLich
                    };
                    _context.LichKiemTraDauVaos.Add(lich);
                    hoSo.TrangThai = TrangThaiHoSo.ChoKiemTraDauVao;
                    await _context.SaveChangesAsync();

                    TempData["ThanhCong"] = "Đã lập lịch kiểm tra đầu vào.";
                    return RedirectToAction(nameof(Index));
                }
            }

            await NapDanhSachHoSo(vm);
            return View(vm);
        }

        // POST: LichKiemTraDauVao/Huy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id)
        {
            var lich = await _context.LichKiemTraDauVaos
                .Include(l => l.HoSoDangKyHoc)
                .FirstOrDefaultAsync(l => l.MaLichKiemTraDauVao == id);
            if (lich == null) return NotFound();

            if (lich.TrangThai != TrangThaiLich.DaLenLich)
            {
                TempData["Loi"] = "Chỉ hủy được lịch đang ở trạng thái \"Đã lên lịch\".";
                return RedirectToAction(nameof(Index));
            }

            lich.TrangThai = TrangThaiLich.DaHuy;
            if (lich.HoSoDangKyHoc != null &&
                lich.HoSoDangKyHoc.TrangThai == TrangThaiHoSo.ChoKiemTraDauVao)
            {
                lich.HoSoDangKyHoc.TrangThai = TrangThaiHoSo.DuDieuKien;
            }

            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã hủy lịch kiểm tra đầu vào.";
            return RedirectToAction(nameof(Index));
        }

        // POST: LichKiemTraDauVao/HoanThanh/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HoanThanh(int id)
        {
            var lich = await _context.LichKiemTraDauVaos
                .Include(l => l.HoSoDangKyHoc)
                .FirstOrDefaultAsync(l => l.MaLichKiemTraDauVao == id);
            if (lich == null) return NotFound();

            if (lich.TrangThai != TrangThaiLich.DaLenLich)
            {
                TempData["Loi"] = "Chỉ hoàn thành được lịch đang ở trạng thái \"Đã lên lịch\".";
                return RedirectToAction(nameof(Index));
            }

            var hoSo = lich.HoSoDangKyHoc;
            if (hoSo == null || hoSo.TrangThai != TrangThaiHoSo.ChoKiemTraDauVao)
            {
                TempData["Loi"] = "Hồ sơ không ở trạng thái \"Chờ kiểm tra đầu vào\", không thể hoàn thành lịch.";
                return RedirectToAction(nameof(Index));
            }

            lich.TrangThai = TrangThaiLich.DaHoanThanh;
            hoSo.TrangThai = TrangThaiHoSo.DaKiemTraDauVao;

            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã hoàn thành lịch. Hồ sơ chuyển sang \"Đã kiểm tra đầu vào\".";
            return RedirectToAction(nameof(Index));
        }

        private async Task NapDanhSachHoSo(LapLichViewModel vm)
        {
            vm.DanhSachHoSo = await _context.HoSoDangKyHocs
                .Where(h => h.TrangThai == TrangThaiHoSo.DuDieuKien &&
                            !h.LichKiemTraDauVaos.Any(l => l.TrangThai != TrangThaiLich.DaHuy))
                .OrderBy(h => h.HocVien!.HoTen)
                .Select(h => new SelectListItem
                {
                    Value = h.MaHoSo.ToString(),
                    Text = h.HocVien!.HoTen + " - " + h.LopHoc!.TenLop + " (HS" + h.MaHoSo + ")",
                    Selected = h.MaHoSo == vm.MaHoSo
                })
                .ToListAsync();
        }
    }
}