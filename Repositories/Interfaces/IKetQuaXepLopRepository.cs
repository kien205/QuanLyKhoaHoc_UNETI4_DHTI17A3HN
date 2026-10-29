// Repository cho KetQuaXepLopController: kết quả xếp lớp và sĩ số lớp.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface IKetQuaXepLopRepository
    {
        Task<List<KetQuaXepLop>> LayTatCaAsync();
        Task<HoSoDangKyHoc?> LayHoSoKemLopAsync(int? maHoSo);
        Task<bool> HoSoDaCoKetQuaAsync(int maHoSo);
        Task<int> DemSoDaXepLopAsync(int maLop);

        /// <summary>Hồ sơ "Đã kiểm tra đầu vào" và chưa có kết quả (kèm học viên, lớp học).</summary>
        Task<List<HoSoDangKyHoc>> LayHoSoChoNhapKetQuaAsync();

        void Them(KetQuaXepLop ketQua);
        Task LuuThayDoiAsync();
    }
}
