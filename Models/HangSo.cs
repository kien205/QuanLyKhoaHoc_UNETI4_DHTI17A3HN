// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models
{
    public class HangSo
    {
        public static class VaiTroTaiKhoan
        {
            public const string Admin = "Admin";
            public const string NhanVienDaoTao = "Nhân viên đào tạo";
            public const string HocVien = "Học viên";
        }

        public static class TrangThaiLop
        {
            public const string ChuaMo = "Chưa mở";
            public const string DangMoDangKy = "Đang mở đăng ký";
            public const string TamDung = "Tạm dừng";
            public const string DaDong = "Đã đóng";
        }

        public static class TrangThaiHoSo
        {
            public const string ChoDuyet = "Chờ duyệt";
            public const string DuDieuKien = "Đủ điều kiện";
            public const string KhongDuDieuKien = "Không đủ điều kiện";
            public const string ChoKiemTraDauVao = "Chờ kiểm tra đầu vào";
            public const string DaKiemTraDauVao = "Đã kiểm tra đầu vào";
            public const string DuocXepLop = "Được xếp lớp";
            public const string ChuaDuocXepLop = "Chưa được xếp lớp";
            public const string DaHuy = "Đã hủy";
        }

        public static class TrangThaiLich
        {
            public const string DaLenLich = "Đã lên lịch";
            public const string DaHoanThanh = "Đã hoàn thành";
            public const string DaHuy = "Đã hủy";
        }

        public static class SessionKeys
        {
            public const string MaTaiKhoan = "MaTaiKhoan";
            public const string HoTen = "HoTen";
            public const string VaiTro = "VaiTro";
        }
    }
}
