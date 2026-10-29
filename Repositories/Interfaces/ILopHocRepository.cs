// Repository cho LopHocController: CRUD, tìm kiếm, lọc, sắp xếp, phân trang lớp học.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface ILopHocRepository
    {
        /// <summary>Lọc + sắp xếp + phân trang. Trang vượt quá tổng số trang sẽ được đưa về trang cuối.</summary>
        Task<(List<LopHoc> DanhSach, int TongBanGhi)> TimKiemAsync(
            string? searchString,
            int? maChuongTrinhDaoTao,
            string? trinhDoDauVao,
            string? trangThai,
            double? diemDauVaoTu,
            string? hanDangKyFilter,
            string? sortOrder,
            int page,
            int pageSize);

        Task<List<ChuongTrinhDaoTao>> LayChuongTrinhDaoTaoAsync(bool chiDangHoatDong);
        Task<List<string>> LayDanhSachTrinhDoDauVaoAsync();

        Task<LopHoc?> LayChiTietAsync(int maLop);
        Task<LopHoc?> TimTheoMaAsync(int maLop);
        Task<bool> TonTaiAsync(int maLop);

        Task<bool> ChuongTrinhDangHoatDongAsync(int maChuongTrinhDaoTao);
        Task<int> DemSoHocVienDaXepLopAsync(int maLop);
        Task<bool> CoHoSoDangKyAsync(int maLop);

        void Them(LopHoc lopHoc);
        /// <summary>Cập nhật và lưu; trả về false nếu bản ghi không còn tồn tại (xung đột đồng thời).</summary>
        Task<bool> CapNhatAsync(LopHoc lopHoc);
        void Xoa(LopHoc lopHoc);
        Task LuuThayDoiAsync();
    }
}
