// Repository cho HoSoXetDuyetController: tìm kiếm, lọc, phân trang và xét duyệt hồ sơ.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IHoSoXetDuyetRepository
    {
        /// <summary>Lọc + sắp xếp + phân trang. Trang vượt quá tổng số trang sẽ được đưa về trang cuối.</summary>
        Task<(List<HoSoDangKyHoc> DanhSach, int TongBanGhi)> TimKiemAsync(
            string? tuKhoa,
            int? maChuongTrinh,
            string? trangThai,
            DateTime? tuNgay,
            DateTime? denNgay,
            string? sapXep,
            int trang,
            int pageSize);

        Task<List<ChuongTrinhDaoTao>> LayDanhSachChuongTrinhAsync();
        Task<HoSoDangKyHoc?> LayChiTietHoSoAsync(int maHoSo);
        Task<int> DemSoDaXepLopAsync(int maLop);
        Task LuuThayDoiAsync();
    }
}
