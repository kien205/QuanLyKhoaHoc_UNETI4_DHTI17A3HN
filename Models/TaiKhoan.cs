// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Entity TaiKhoan.
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập phải từ 4 đến 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = string.Empty;

        [Display(Name = "Hoạt động")]
        public bool TrangThai { get; set; } = true;

        public HocVien? HocVien { get; set; }
    }
}