// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Dashboard 9 chỉ số (Index) và 10 thống kê LINQ (ChiTiet).

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao)]
    public class ThongKeController : Controller
    {
        private readonly AppDbContext _context;

        public ThongKeController(AppDbContext context)
        {
            _context = context;
        }

        private static double TyLe(int tu, int mau)
            => mau == 0 ? 0 : Math.Round(tu * 100.0 / mau, 1);

        private static ThongKeLopViewModel Dong(string nhan, int soLuong)
            => new ThongKeLopViewModel { TenLop = nhan, TongHoSo = soLuong };

        // ===== DASHBOARD: 9 chỉ số =====
        public async Task<IActionResult> Index()
        {
            var bayGio = DateTime.Now;
            var vm = new DashboardViewModel
            {
                TongChuongTrinh = await _context.ChuongTrinhDaoTaos.CountAsync(),
                TongLop = await _context.LopHocs.CountAsync(),
                LopDangMo = await _context.LopHocs.CountAsync(l => l.TrangThai == TrangThaiLop.DangMoDangKy),
                TongHocVien = await _context.HocViens.CountAsync(),
                TongHoSo = await _context.HoSoDangKyHocs.CountAsync(),
                HoSoChoDuyet = await _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == TrangThaiHoSo.ChoDuyet),
                HoSoDuDieuKien = await _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == TrangThaiHoSo.DuDieuKien),
                LichSapToi = await _context.LichKiemTraDauVaos.CountAsync(l =>
                    l.TrangThai == TrangThaiLich.DaLenLich && l.ThoiGianBatDau >= bayGio),
                HoSoDuocXepLop = await _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == TrangThaiHoSo.DuocXepLop)
            };
            return View(vm);
        }

        // ===== THỐNG KÊ: 10 mục =====
        public async Task<IActionResult> ChiTiet()
        {
            var vm = new ThongKeChiTietViewModel();

            var tk1 = await _context.ChuongTrinhDaoTaos
                .OrderBy(c => c.TenChuongTrinhDaoTao)
                .Select(c => new { Nhan = c.TenChuongTrinhDaoTao, SoLuong = c.LopHocs.Count() })
                .ToListAsync();
            vm.LopTheoChuongTrinh = tk1.Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            var daQuaDuyet = new[]
            {
                TrangThaiHoSo.DuDieuKien, TrangThaiHoSo.ChoKiemTraDauVao, TrangThaiHoSo.DaKiemTraDauVao,
                TrangThaiHoSo.DuocXepLop, TrangThaiHoSo.ChuaDuocXepLop
            };
            var daXetDuyet = daQuaDuyet.Append(TrangThaiHoSo.KhongDuDieuKien).ToArray();

            var theoLop = await _context.HoSoDangKyHocs
                .GroupBy(h => h.MaLop)
                .Select(g => new
                {
                    MaLop = g.Key,
                    Tong = g.Count(),
                    QuaDuyet = g.Count(h => daQuaDuyet.Contains(h.TrangThai)),
                    DaXet = g.Count(h => daXetDuyet.Contains(h.TrangThai))
                })
                .ToListAsync();

            var cacLop = await _context.LopHocs
                .OrderBy(l => l.TenLop)
                .Select(l => new { l.MaLop, l.TenLop })
                .ToListAsync();

            vm.HoSoTheoLop = cacLop.Select(l =>
            {
                var x = theoLop.FirstOrDefault(t => t.MaLop == l.MaLop);
                int quaDuyet = x?.QuaDuyet ?? 0;
                int daXet = x?.DaXet ?? 0;
                return new ThongKeLopViewModel
                {
                    TenLop = l.TenLop,
                    TongHoSo = x?.Tong ?? 0,
                    DaQuaDuyet = quaDuyet,
                    DaXetDuyet = daXet,
                    TyLeQuaDuyet = TyLe(quaDuyet, daXet)
                };
            }).ToList();

            var tk3 = await _context.HoSoDangKyHocs
                .GroupBy(h => h.TrangThai)
                .Select(g => new { Nhan = g.Key, SoLuong = g.Count() })
                .ToListAsync();
            vm.HoSoTheoTrangThai = tk3
                .OrderByDescending(x => x.SoLuong)
                .Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            int max = vm.HoSoTheoLop.Any() ? vm.HoSoTheoLop.Max(l => l.TongHoSo) : 0;
            vm.LopNhieuHoSoNhat = max == 0
                ? new List<ThongKeLopViewModel>()
                : vm.HoSoTheoLop.Where(l => l.TongHoSo == max)
                    .Select(l => Dong(l.TenLop, l.TongHoSo)).ToList();

            var tk6 = await _context.ChuongTrinhDaoTaos
                .OrderBy(c => c.TenChuongTrinhDaoTao)
                .Select(c => new
                {
                    Nhan = c.TenChuongTrinhDaoTao,
                    SoLuong = _context.HoSoDangKyHocs.Count(h =>
                        h.LopHoc!.MaChuongTrinhDaoTao == c.MaChuongTrinhDaoTao &&
                        h.TrangThai == TrangThaiHoSo.DuocXepLop)
                })
                .ToListAsync();
            vm.XepLopTheoChuongTrinh = tk6.Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            var hoSoThang = await _context.HoSoDangKyHocs
                .GroupBy(h => new { h.NgayNop.Year, h.NgayNop.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, SoLuong = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();
            vm.HoSoTheoThang = hoSoThang
                .Select(x => Dong($"{x.Month:00}/{x.Year}", x.SoLuong)).ToList();

            var lichThang = await _context.LichKiemTraDauVaos
                .GroupBy(l => new { l.ThoiGianBatDau.Year, l.ThoiGianBatDau.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, SoLuong = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();
            vm.LichTheoThang = lichThang
                .Select(x => Dong($"{x.Month:00}/{x.Year}", x.SoLuong)).ToList();

            vm.SoDuocXepLop = await _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == TrangThaiHoSo.DuocXepLop);
            int chuaXep = await _context.HoSoDangKyHocs.CountAsync(h => h.TrangThai == TrangThaiHoSo.ChuaDuocXepLop);
            vm.SoDaKiemTra = vm.SoDuocXepLop + chuaXep;
            vm.TyLeXepLop = TyLe(vm.SoDuocXepLop, vm.SoDaKiemTra);

            return View(vm);
        }
    }
}