// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Entity ChuongTrinhDaoTao.
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class ChuongTrinhDaoTao
    {
        [Key]
        public int MaChuongTrinhDaoTao { get; set; }

        [Required(ErrorMessage = "Tên chương trình đào tạo không được để trống")]
        [StringLength(150, ErrorMessage = "Tên tối đa 150 ký tự")]
        [Display(Name = "Tên chương trình đào tạo")]
        public string TenChuongTrinhDaoTao { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự")]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        [Display(Name = "Email liên hệ")]
        public string? EmailLienHe { get; set; }

        [Display(Name = "Hoạt động")]
        public bool TrangThai { get; set; } = true;

        public ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();
    }
}