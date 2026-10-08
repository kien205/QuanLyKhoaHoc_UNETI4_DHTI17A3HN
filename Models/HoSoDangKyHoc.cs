// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using System.ComponentModel.DataAnnotations;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class HoSoDangKyHoc
    {
        [Key] 
        public int MaHoSo { get; set; }
        public int MaHocVien { get; set; }
        public HocVien? HocVien { get; set; }
        public int MaLop { get; set; }
        public LopHoc? LopHoc { get; set; }
        public DateTime NgayNop { get; set; }
        [StringLength(1000)] 
        public string? LyDoDangKy { get; set; }
        [StringLength(1000)] 
        public string? GhiChu { get; set; }
        [Required, StringLength(30)] 
        public string TrangThai { get; set; } = TrangThaiHoSo.ChoDuyet;
        public DateTime? NgayXuLy { get; set; }
        [StringLength(1000)] 
        public string? NhanXetXetDuyet { get; set; }
        public ICollection<LichKiemTraDauVao> LichKiemTraDauVaos { get; set; } = new List<LichKiemTraDauVao>();
        public KetQuaXepLop? KetQuaXepLop { get; set; }
    }
}
