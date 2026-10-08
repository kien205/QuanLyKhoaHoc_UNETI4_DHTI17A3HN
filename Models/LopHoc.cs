// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class LopHoc
    {
        [Key]
        public int MaLop { get; set; }
        [Required]
        [StringLength(50)]
        public string TenLop { get; set; } = string.Empty;

        public int MaChuongTrinhDaoTao { get; set; }
        public ChuongTrinhDaoTao? ChuongTrinhDaoTao { get; set; }

        [Range(1,50)]
        public int SiSoToiDa { get; set; }

        [Required]
        [StringLength(50)]
        public string TrinhDoDauVao { get; set; } = string.Empty;

        [Range(0,10)]
        public double DiemDauVao { get; set; }
        public DateTime NgayBatDauDangKy {  get; set; }
        public DateTime HanDangKy { get; set; }

        [StringLength(2000)]
        public string MoTaLopHoc { get; set; } = string.Empty;

        [StringLength(2000)]
        public string YeuCauHocVien { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string TrangThai {  get; set; } = string.Empty;

        public ICollection<HoSoDangKyHoc> hoSoDangKyHoc { get; set; } = new List<HoSoDangKyHoc>();
    }
}
