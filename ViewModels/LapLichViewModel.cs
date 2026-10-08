// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Lịch kiểm tra đầu vào (ViewModel lập lịch)

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class LapLichViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn hồ sơ.")]
        [Display(Name = "Hồ sơ đăng ký")]
        public int? MaHoSo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu.")]
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGianBatDau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc.")]
        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGianKetThuc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập hình thức kiểm tra.")]
        [StringLength(50)]
        [Display(Name = "Hình thức kiểm tra")]
        public string HinhThucKiemTra { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Địa điểm hoặc liên kết")]
        public string? DiaDiemHoacLienKet { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giáo viên kiểm tra.")]
        [StringLength(100)]
        [Display(Name = "Giáo viên kiểm tra")]
        public string GiaoVienKiemTra { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public IEnumerable<SelectListItem>? DanhSachHoSo { get; set; }
    }
}