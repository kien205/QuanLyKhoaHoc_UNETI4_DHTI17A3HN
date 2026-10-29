// Repository cho DangKyHocController: truy vấn lớp đang mở và hồ sơ đăng ký của học viên.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IDangKyHocRepository
    {
        Task<List<LopHoc>> LayLopDangMoDangKyAsync();
        Task<LopHoc?> LayLopKemChuongTrinhAsync(int maLop);
        Task<bool> LopConNhanHoSoAsync(int maLop, DateTime homNay);

        Task<List<int>> LayMaLopCoHoSoDangXuLyAsync(int maHocVien, int[] maLops, string[] trangThaiDangXuLy);
        Task<bool> CoHoSoDangXuLyAsync(int maHocVien, int maLop, string[] trangThaiDangXuLy);

        Task<List<HoSoDangKyHoc>> LayHoSoCuaHocVienAsync(int maHocVien);
        Task<HoSoDangKyHoc?> LayHoSoCuaHocVienDeHuyAsync(int maHoSo, int maHocVien);

        void ThemHoSo(HoSoDangKyHoc hoSo);
        Task LuuThayDoiAsync();
    }
}
