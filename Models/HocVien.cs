using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class HocVien
    {
        [Key] 
        public int MaHocVien { get; set; }
        public int MaTaiKhoan { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
        [Required, StringLength(100)] 
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        [StringLength(10)] 
        public string GioiTinh { get; set; } = string.Empty;
        [Phone, StringLength(15)] 
        public string? SoDienThoai { get; set; }
        [EmailAddress, StringLength(100)] 
        public string? Email { get; set; }
        [StringLength(255)] 
        public string? DiaChi { get; set; }
        [StringLength(50)] 
        public string? TrinhDoHienTai { get; set; }
        [StringLength(100)] 
        public string? NgheNghiep { get; set; }
        [Range(0, int.MaxValue)] 
        public int SoThangDaHoc { get; set; }
        [StringLength(1000)] 
        public string? MucTieuHoc { get; set; }
        public bool TrangThai { get; set; } = true;
        public ICollection<HoSoDangKyHoc> HoSoDangKyHocs { get; set; } = new List<HoSoDangKyHoc>();
    }
}
