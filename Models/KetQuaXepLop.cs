using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class KetQuaXepLop
    {
        [Key] 
        public int MaKetQuaXepLop { get; set; }
        public int MaHoSo { get; set; }
        public HoSoDangKyHoc? HoSoDangKyHoc { get; set; }
        [Range(0, 100)] 
        public double? DiemDanhGia { get; set; }
        [StringLength(1000)] 
        public string? NhanXet { get; set; }
        [Required, StringLength(30)] 
        public string KetQua { get; set; } = string.Empty;
        public DateTime NgayCapNhat { get; set; }
    }
}
