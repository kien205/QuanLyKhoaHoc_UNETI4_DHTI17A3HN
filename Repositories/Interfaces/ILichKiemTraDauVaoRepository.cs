// Repository cho LichKiemTraDauVaoController: lập lịch, kiểm tra trùng lịch, hủy/hoàn thành lịch.
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces
{
    public interface ILichKiemTraDauVaoRepository
    {
        Task<List<LichKiemTraDauVao>> LayTatCaAsync();
        Task<LichKiemTraDauVao?> LayLichKemHoSoAsync(int maLich);
        Task<HoSoDangKyHoc?> LayHoSoAsync(int? maHoSo);

        Task<bool> HoSoCoLichHieuLucAsync(int maHoSo);
        Task<bool> TrungLichHocVienAsync(int maHocVien, DateTime batDau, DateTime ketThuc);
        Task<bool> TrungLichGiaoVienAsync(string giaoVien, DateTime batDau, DateTime ketThuc);

        /// <summary>Hồ sơ "Đủ điều kiện" chưa có lịch còn hiệu lực (kèm học viên, lớp học).</summary>
        Task<List<HoSoDangKyHoc>> LayHoSoChoLapLichAsync();

        void Them(LichKiemTraDauVao lich);
        Task LuuThayDoiAsync();
    }
}
