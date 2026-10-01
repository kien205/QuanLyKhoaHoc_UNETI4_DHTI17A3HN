// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: ViewModel danh sách lớp học cho học viên đăng ký.

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class LopHocDangKyViewModel
    {
        public int MaLop { get; set; }
        public string TenLop { get; set; } = string.Empty;
        public string TenChuongTrinhDaoTao { get; set; } = string.Empty;
        public string TrinhDoDauVao { get; set; } = string.Empty;
        public double DiemDauVao { get; set; }
        public DateTime NgayBatDauDangKy { get; set; }
        public DateTime HanDangKy { get; set; }
        public bool DangTrongHanDangKy { get; set; }
        public bool DaCoHoSoDangXuLy { get; set; }
    }
}
