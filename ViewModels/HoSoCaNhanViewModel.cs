// Họ và tên: Nguyễn Văn Giáp
// Mã sinh viên: 22103100110
// Nội dung thực hiện: ViewModel hồ sơ cá nhân học viên.

using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class HoSoCaNhanViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [StringLength(50)]
        [Display(Name = "Trình độ hiện tại")]
        public string? TrinhDoHienTai { get; set; }

        [StringLength(100)]
        [Display(Name = "Nghề nghiệp")]
        public string? NgheNghiep { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số tháng đã học không được âm.")]
        [Display(Name = "Số tháng đã học")]
        public int SoThangDaHoc { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mục tiêu học")]
        public string? MucTieuHoc { get; set; }
    }
}
