// Repository cho TaiKhoanController: đăng nhập và quản lý tài khoản.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface ITaiKhoanRepository
    {
        TaiKhoan? LayTheoTenDangNhap(string tenDangNhap);
        Task<List<TaiKhoan>> TimKiemAsync(string? tuKhoa, string? vaiTro);
        Task<bool> TenDangNhapTonTaiAsync(string tenDangNhap);
        Task<TaiKhoan?> TimTheoMaAsync(int maTaiKhoan);
        Task<HocVien?> LayHocVienTheoTaiKhoanAsync(int maTaiKhoan, bool theoDoiThayDoi = false);

        void Them(TaiKhoan taiKhoan);
        void ThemHocVien(HocVien hocVien);
        Task LuuThayDoiAsync();
    }
}
