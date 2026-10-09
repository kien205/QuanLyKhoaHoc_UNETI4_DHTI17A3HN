// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: ViewModel quản lý tài khoản.
using System.ComponentModel.DataAnnotations;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class TaiKhoanCreateViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = "Chỉ gồm chữ, số, dấu _ và dấu .")]
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

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = VaiTroTaiKhoan.HocVien;
    }

    public class TaiKhoanEditViewModel
    {
        public int MaTaiKhoan { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;   // chỉ hiển thị, không cho đổi

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = string.Empty;

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu mới phải từ 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới (để trống nếu không đổi)")]
        public string? MatKhauMoi { get; set; }
    }
}