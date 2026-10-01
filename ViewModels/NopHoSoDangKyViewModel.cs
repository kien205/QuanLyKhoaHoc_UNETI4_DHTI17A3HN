// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: ViewModel nộp hồ sơ đăng ký học.

using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class NopHoSoDangKyViewModel
    {
        [StringLength(1000, ErrorMessage = "Lý do đăng ký không được vượt quá 1000 ký tự.")]
        [Display(Name = "Lý do đăng ký")]
        public string? LyDoDangKy { get; set; }
    }
}
