// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class ChuongTrinhDaoTao
    {
        [Key]
        public int MaChuongTrinhDaoTao {  get; set; }

        [Required, StringLength(150)] 
        public string TenChuongTrinhDaoTao { get; set; } = string.Empty;

        [StringLength(2000)] public string? MoTa { get; set; }
        [StringLength(100)] 
        public string? EmailLienHe { get; set; }

        [Display(Name = "Hoạt động")]
        public bool TrangThai { get; set; } = true;
        public ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();

    }
}
