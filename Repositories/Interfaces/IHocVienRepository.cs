// Repository cho HocVienController (và lớp cha HocVienModuleController): thao tác CSDL với hồ sơ học viên.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IHocVienRepository
    {
        /// <summary>Lấy học viên gắn với tài khoản; theoDoiThayDoi = true khi cần sửa rồi lưu lại.</summary>
        Task<HocVien?> LayTheoMaTaiKhoanAsync(int maTaiKhoan, bool theoDoiThayDoi = false);
        Task LuuThayDoiAsync();
    }
}
