// Repository cho ChuongTrinhDaoTaoController: khai báo các thao tác CSDL với bảng ChuongTrinhDaoTao.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IChuongTrinhDaoTaoRepository
    {
        Task<List<ChuongTrinhDaoTao>> LayTatCaKemLopHocAsync();
        Task<ChuongTrinhDaoTao?> LayChiTietKemLopHocAsync(int maChuongTrinh);
        Task<ChuongTrinhDaoTao?> TimTheoMaAsync(int maChuongTrinh);

        /// <summary>Kiểm tra tên đã tồn tại; boQuaMa dùng khi sửa để bỏ qua chính bản ghi đang sửa.</summary>
        Task<bool> TrungTenAsync(string tenChuongTrinh, int? boQuaMa = null);
        Task<bool> CoLopHocAsync(int maChuongTrinh);

        void Them(ChuongTrinhDaoTao ctdt);
        /// <summary>Cập nhật và lưu; trả về false nếu bản ghi không còn tồn tại (xung đột đồng thời).</summary>
        Task<bool> CapNhatAsync(ChuongTrinhDaoTao ctdt);
        void Xoa(ChuongTrinhDaoTao ctdt);
        Task LuuThayDoiAsync();
    }
}
