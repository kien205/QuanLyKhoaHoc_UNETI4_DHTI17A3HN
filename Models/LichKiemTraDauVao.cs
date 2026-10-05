// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using System.ComponentModel.DataAnnotations;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class LichKiemTraDauVao
    {
        [Key] 
        public int MaLichKiemTraDauVao { get; set; }
        public int MaHoSo { get; set; }
        public HoSoDangKyHoc? HoSoDangKyHoc { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        [StringLength(50)] 
        public string HinhThucKiemTra { get; set; } = string.Empty;
        [StringLength(255)] 
        public string? DiaDiemHoacLienKet { get; set; }
        [Required, StringLength(100)] 
        public string GiaoVienKiemTra { get; set; } = string.Empty;
        [StringLength(1000)] 
        public string? GhiChu { get; set; }
        [Required, StringLength(30)] 
        public string TrangThai { get; set; } = TrangThaiLich.DaLenLich;
    }
}
