// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: ViewModel theo dõi hồ sơ và lịch kiểm tra của học viên.

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class HoSoHocVienViewModel
    {
        public int MaHoSo { get; set; }
        public string TenLop { get; set; } = string.Empty;
        public string TenChuongTrinhDaoTao { get; set; } = string.Empty;
        public DateTime NgayNop { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string? KetQuaXepLop { get; set; }
        public bool CoTheHuy { get; set; }
        public List<LichHocVienViewModel> LichKiemTra { get; set; } = new();
    }

    public class LichHocVienViewModel
    {
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string HinhThucKiemTra { get; set; } = string.Empty;
        public string? DiaDiemHoacLienKet { get; set; }
        public string GiaoVienKiemTra { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
    }
}
