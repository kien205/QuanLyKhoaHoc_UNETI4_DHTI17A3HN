// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Kết quả xếp lớp: nhập kết quả, cập nhật đồng bộ
// trạng thái hồ sơ, kiểm tra sĩ số tối đa của lớp.

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
    public class KetQuaXepLopController : Controller
    {
        private readonly AppDbContext _context;

        public KetQuaXepLopController(AppDbContext context)
        {
            _context = context;
        }

        // GET: KetQuaXepLop
        public async Task<IActionResult> Index()
        {
            var ds = await _context.KetQuaXepLops
                .Include(k => k.HoSoDangKyHoc!).ThenInclude(h => h.HocVien)
                .Include(k => k.HoSoDangKyHoc!).ThenInclude(h => h.LopHoc)
                .OrderByDescending(k => k.NgayCapNhat)
                .ToListAsync();
            return View(ds);
        }

        // GET: KetQuaXepLop/Create
        public async Task<IActionResult> Create(int? maHoSo)
        {
            var vm = new NhapKetQuaViewModel { MaHoSo = maHoSo };
            await NapDanhSachHoSo(vm);
            return View(vm);
        }

        // POST: KetQuaXepLop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhapKetQuaViewModel vm)
        {
            if (vm.KetQua != TrangThaiHoSo.DuocXepLop &&
                vm.KetQua != TrangThaiHoSo.ChuaDuocXepLop)
            {
                ModelState.AddModelError(nameof(vm.KetQua), "Kết quả không hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                var hoSo = await _context.HoSoDangKyHocs
                    .Include(h => h.LopHoc)
                    .FirstOrDefaultAsync(h => h.MaHoSo == vm.MaHoSo);

                if (hoSo == null)
                {
                    ModelState.AddModelError(nameof(vm.MaHoSo), "Hồ sơ không tồn tại.");
                }
                else
                {
                    if (hoSo.TrangThai != TrangThaiHoSo.DaKiemTraDauVao)
                    {
                        ModelState.AddModelError(nameof(vm.MaHoSo),
                            "Chỉ hồ sơ ở trạng thái \"Đã kiểm tra đầu vào\" mới được nhập kết quả.");
                    }

                    if (await _context.KetQuaXepLops.AnyAsync(k => k.MaHoSo == hoSo.MaHoSo))
                    {
                        ModelState.AddModelError(nameof(vm.MaHoSo),
                            "Hồ sơ này đã có kết quả xếp lớp.");
                    }

                    if (vm.KetQua == TrangThaiHoSo.DuocXepLop && hoSo.LopHoc != null)
                    {
                        int daXep = await _context.HoSoDangKyHocs.CountAsync(h =>
                            h.MaLop == hoSo.MaLop &&
                            h.TrangThai == TrangThaiHoSo.DuocXepLop);

                        if (daXep >= hoSo.LopHoc.SiSoToiDa)
                        {
                            ModelState.AddModelError(nameof(vm.KetQua),
                                $"Lớp \"{hoSo.LopHoc.TenLop}\" đã đủ sĩ số tối đa, không thể xếp thêm.");
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.KetQuaXepLops.Add(new KetQuaXepLop
                    {
                        MaHoSo = hoSo!.MaHoSo,
                        DiemDanhGia = vm.DiemDanhGia,
                        NhanXet = vm.NhanXet?.Trim(),
                        KetQua = vm.KetQua,
                        NgayCapNhat = DateTime.Now,
                    });

                    hoSo.TrangThai = vm.KetQua;

                    await _context.SaveChangesAsync();

                    string thongBao = "Đã ghi nhận kết quả xếp lớp.";

                    if (vm.KetQua == TrangThaiHoSo.DuocXepLop && hoSo.LopHoc != null)
                    {
                        int daXepSau = await _context.HoSoDangKyHocs.CountAsync(h =>
                            h.MaLop == hoSo.MaLop &&
                            h.TrangThai == TrangThaiHoSo.DuocXepLop);
                        if (daXepSau >= hoSo.LopHoc.SiSoToiDa)
                            thongBao += $" Cảnh báo: lớp \"{hoSo.LopHoc.TenLop}\" đã đạt sĩ số tối đa, có thể đóng lớp.";
                    }

                    TempData["ThanhCong"] = thongBao;
                    return RedirectToAction(nameof(Index));
                }
            }

            await NapDanhSachHoSo(vm);
            return View(vm);
        }

        private async Task NapDanhSachHoSo(NhapKetQuaViewModel vm)
        {
            vm.DanhSachHoSo = await _context.HoSoDangKyHocs
                .Where(h => h.TrangThai == TrangThaiHoSo.DaKiemTraDauVao &&
                            h.KetQuaXepLop == null)
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