// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength (50,MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string MatKhau {  get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string VaiTro { get; set; } = string.Empty;


        public bool TrangThai { get; set; } = true;

        public HocVien? HocVien { get; set; }
    }
}
