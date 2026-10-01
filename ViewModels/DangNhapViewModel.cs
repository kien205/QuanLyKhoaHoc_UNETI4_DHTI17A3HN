using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;
    }
}
