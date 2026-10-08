// Họ và tên: Lê Đức Long
// Mã sinh viên: 23103100015
// Nội dung thực hiện: Kết quả xếp lớp (ViewModel nhập kết quả)

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class NhapKetQuaViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn hồ sơ.")]
        [Display(Name = "Hồ sơ đăng ký")]
        public int? MaHoSo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm đánh giá.")]
        [Range(0, 100, ErrorMessage = "Điểm đánh giá phải từ 0 đến 100.")]
        [Display(Name = "Điểm đánh giá (0-100)")]
        public double? DiemDanhGia { get; set; }

        [StringLength(1000)]
        [Display(Name = "Nhận xét")]
        public string? NhanXet { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn kết quả.")]
        [Display(Name = "Kết quả")]
        public string KetQua { get; set; } = string.Empty;
        public IEnumerable<SelectListItem>? DanhSachHoSo { get; set; }
    }
}