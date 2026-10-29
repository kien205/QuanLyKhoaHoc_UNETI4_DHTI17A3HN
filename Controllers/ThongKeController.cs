// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Dashboard 9 chỉ số (Index) và 10 thống kê LINQ (ChiTiet).
// Truy vấn CSDL được tách sang IThongKeRepository.

using Microsoft.AspNetCore.Mvc;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao)]
    public class ThongKeController : Controller
    {
        private readonly IThongKeRepository _repo;

        public ThongKeController(IThongKeRepository repo)
        {
            _repo = repo;
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
                TongChuongTrinh = await _repo.DemChuongTrinhAsync(),
                TongLop = await _repo.DemLopAsync(),
                LopDangMo = await _repo.DemLopAsync(TrangThaiLop.DangMoDangKy),
                TongHocVien = await _repo.DemHocVienAsync(),
                TongHoSo = await _repo.DemHoSoAsync(),
                HoSoChoDuyet = await _repo.DemHoSoAsync(TrangThaiHoSo.ChoDuyet),
                HoSoDuDieuKien = await _repo.DemHoSoAsync(TrangThaiHoSo.DuDieuKien),
                LichSapToi = await _repo.DemLichSapToiAsync(bayGio),
                HoSoDuocXepLop = await _repo.DemHoSoAsync(TrangThaiHoSo.DuocXepLop)
            };
            return View(vm);
        }

        // ===== THỐNG KÊ: 10 mục =====
        public async Task<IActionResult> ChiTiet()
        {
            var vm = new ThongKeChiTietViewModel();

            var tk1 = await _repo.DemLopTheoChuongTrinhAsync();
            vm.LopTheoChuongTrinh = tk1.Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            var daQuaDuyet = new[]
            {
                TrangThaiHoSo.DuDieuKien, TrangThaiHoSo.ChoKiemTraDauVao, TrangThaiHoSo.DaKiemTraDauVao,
                TrangThaiHoSo.DuocXepLop, TrangThaiHoSo.ChuaDuocXepLop
            };
            var daXetDuyet = daQuaDuyet.Append(TrangThaiHoSo.KhongDuDieuKien).ToArray();

            var theoLop = await _repo.ThongKeHoSoTheoLopAsync(daQuaDuyet, daXetDuyet);
            var cacLop = await _repo.LayDanhSachLopAsync();

            vm.HoSoTheoLop = cacLop.Select(l =>
            {
                // Lớp chưa có hồ sơ: FirstOrDefault trả về tuple mặc định (toàn số 0).
                var x = theoLop.FirstOrDefault(t => t.MaLop == l.MaLop);
                int quaDuyet = x.QuaDuyet;
                int daXet = x.DaXet;
                return new ThongKeLopViewModel
                {
                    TenLop = l.TenLop,
                    TongHoSo = x.Tong,
                    DaQuaDuyet = quaDuyet,
                    DaXetDuyet = daXet,
                    TyLeQuaDuyet = TyLe(quaDuyet, daXet)
                };
            }).ToList();

            var tk3 = await _repo.DemHoSoTheoTrangThaiAsync();
            vm.HoSoTheoTrangThai = tk3
                .OrderByDescending(x => x.SoLuong)
                .Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            int max = vm.HoSoTheoLop.Any() ? vm.HoSoTheoLop.Max(l => l.TongHoSo) : 0;
            vm.LopNhieuHoSoNhat = max == 0
                ? new List<ThongKeLopViewModel>()
                : vm.HoSoTheoLop.Where(l => l.TongHoSo == max)
                    .Select(l => Dong(l.TenLop, l.TongHoSo)).ToList();

            var tk6 = await _repo.DemXepLopTheoChuongTrinhAsync();
            vm.XepLopTheoChuongTrinh = tk6.Select(x => Dong(x.Nhan, x.SoLuong)).ToList();

            var hoSoThang = await _repo.DemHoSoTheoThangAsync();
            vm.HoSoTheoThang = hoSoThang
                .Select(x => Dong($"{x.Thang:00}/{x.Nam}", x.SoLuong)).ToList();

            var lichThang = await _repo.DemLichTheoThangAsync();
            vm.LichTheoThang = lichThang
                .Select(x => Dong($"{x.Thang:00}/{x.Nam}", x.SoLuong)).ToList();

            vm.SoDuocXepLop = await _repo.DemHoSoAsync(TrangThaiHoSo.DuocXepLop);
            int chuaXep = await _repo.DemHoSoAsync(TrangThaiHoSo.ChuaDuocXepLop);
            vm.SoDaKiemTra = vm.SoDuocXepLop + chuaXep;
            vm.TyLeXepLop = TyLe(vm.SoDuocXepLop, vm.SoDaKiemTra);

            return View(vm);
        }
    }
}
