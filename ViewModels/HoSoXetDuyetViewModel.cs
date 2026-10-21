// Họ và tên: Tạ Thị Bích Loan 
// Mã sinh viên: 23103100147
// Nội dung thực hiện: ViewModel Module 4 - Quản lý lọc, tìm kiếm, phân trang và xét duyệt hồ sơ
// Sửa kiểm tra đầu vào xét duyệt: Nguyễn Văn Giáp, 22103100110.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class DanhSachHoSoViewModel
    {
        public List<HoSoDangKyHoc> DanhSachHoSo { get; set; } = new List<HoSoDangKyHoc>();

        public string? TuKhoa { get; set; }
        public int? MaChuongTrinhDaoTao { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string? SapXep { get; set; }

        public int TrangHienTai { get; set; } = 1;
        public int TongSoTrang { get; set; }
        public int TongSoBanGhi { get; set; }
        public int KichThuocTrang { get; set; } = 10;
    }

    public class XetDuyetHoSoViewModel
    {
        public int MaHoSo { get; set; }
        public HoSoDangKyHoc? HoSo { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn kết quả xét duyệt.")]
        public string KetQuaDuyet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nhận xét / lý do xét duyệt.")]
        [StringLength(1000, ErrorMessage = "Nhận xét không được vượt quá 1000 ký tự.")]
        public string? NhanXetXetDuyet { get; set; }
        public int SiSoToiDa { get; set; }
        public int SoLuongDaXepLop { get; set; }
        public bool DaDaySiSo => SoLuongDaXepLop >= SiSoToiDa;
    }
}