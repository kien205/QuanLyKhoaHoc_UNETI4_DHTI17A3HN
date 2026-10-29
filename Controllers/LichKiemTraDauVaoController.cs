// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Lịch kiểm tra đầu vào: lập lịch, kiểm tra trùng lịch,
// hủy lịch, hoàn thành lịch.
// Truy vấn CSDL được tách sang ILichKiemTraDauVaoRepository.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao)]
    public class LichKiemTraDauVaoController : Controller
    {
        private readonly ILichKiemTraDauVaoRepository _repo;

        public LichKiemTraDauVaoController(ILichKiemTraDauVaoRepository repo)
        {
            _repo = repo;
        }

        // GET: LichKiemTraDauVao
        public async Task<IActionResult> Index()
        {
            var ds = await _repo.LayTatCaAsync();
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

                var hoSo = await _repo.LayHoSoAsync(vm.MaHoSo);
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
                    bool daCoLich = await _repo.HoSoCoLichHieuLucAsync(hoSo.MaHoSo);
                    if (daCoLich)
                    {
                        ModelState.AddModelError(nameof(vm.MaHoSo),
                            "Hồ sơ này đã có lịch kiểm tra đầu vào còn hiệu lực.");
                    }

                    bool trungHocVien = await _repo.TrungLichHocVienAsync(hoSo.MaHocVien, batDau, ketThuc);
                    if (trungHocVien)
                    {
                        ModelState.AddModelError(nameof(vm.ThoiGianBatDau),
                            "Học viên đã có lịch kiểm tra đầu vào khác trùng khoảng thời gian này.");
                    }

                    bool trungGiaoVien = await _repo.TrungLichGiaoVienAsync(vm.GiaoVienKiemTra, batDau, ketThuc);
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
                    _repo.Them(lich);
                    hoSo.TrangThai = TrangThaiHoSo.ChoKiemTraDauVao;
                    await _repo.LuuThayDoiAsync();

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
            var lich = await _repo.LayLichKemHoSoAsync(id);
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

            await _repo.LuuThayDoiAsync();
            TempData["ThanhCong"] = "Đã hủy lịch kiểm tra đầu vào.";
            return RedirectToAction(nameof(Index));
        }

        // POST: LichKiemTraDauVao/HoanThanh/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HoanThanh(int id)
        {
            var lich = await _repo.LayLichKemHoSoAsync(id);
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

            await _repo.LuuThayDoiAsync();
            TempData["ThanhCong"] = "Đã hoàn thành lịch. Hồ sơ chuyển sang \"Đã kiểm tra đầu vào\".";
            return RedirectToAction(nameof(Index));
        }

        private async Task NapDanhSachHoSo(LapLichViewModel vm)
        {
            var hoSos = await _repo.LayHoSoChoLapLichAsync();
            vm.DanhSachHoSo = hoSos
                .Select(h => new SelectListItem
                {
                    Value = h.MaHoSo.ToString(),
                    Text = h.HocVien?.HoTen + " - " + h.LopHoc?.TenLop + " (HS" + h.MaHoSo + ")",
                    Selected = h.MaHoSo == vm.MaHoSo
                })
                .ToList();
        }
    }
}
