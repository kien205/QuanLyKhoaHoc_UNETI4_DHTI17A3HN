// Repository cho ThongKeController: các truy vấn đếm/nhóm phục vụ dashboard và thống kê.
namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IThongKeRepository
    {
        // ===== Dashboard =====
        Task<int> DemChuongTrinhAsync();
        Task<int> DemLopAsync(string? trangThai = null);
        Task<int> DemHocVienAsync();
        Task<int> DemHoSoAsync(string? trangThai = null);
        Task<int> DemLichSapToiAsync(DateTime tuThoiDiem);

        // ===== Thống kê chi tiết =====
        Task<List<(string Nhan, int SoLuong)>> DemLopTheoChuongTrinhAsync();
        Task<List<(int MaLop, int Tong, int QuaDuyet, int DaXet)>> ThongKeHoSoTheoLopAsync(
            string[] trangThaiQuaDuyet, string[] trangThaiDaXetDuyet);
        Task<List<(int MaLop, string TenLop)>> LayDanhSachLopAsync();
        Task<List<(string Nhan, int SoLuong)>> DemHoSoTheoTrangThaiAsync();
        Task<List<(string Nhan, int SoLuong)>> DemXepLopTheoChuongTrinhAsync();
        Task<List<(int Nam, int Thang, int SoLuong)>> DemHoSoTheoThangAsync();
        Task<List<(int Nam, int Thang, int SoLuong)>> DemLichTheoThangAsync();
    }
}
