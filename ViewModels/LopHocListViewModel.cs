// Họ và tên: Phan Thị Hương Trà
// Mã sinh viên: 23103100139
// Nội dung thực hiện: Xây dựng ViewModel phục vụ hiển thị danh sách lớp học, tìm kiếm, lọc, sắp xếp và phân trang.

using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class LopHocListViewModel
    {
        public List<LopHoc> LopHocs { get; set; } = new();

        public string? SearchString { get; set; }

        public int? MaChuongTrinhDaoTao { get; set; }

        public string? TrinhDoDauVao { get; set; }

        public string? TrangThai { get; set; }

        public double? DiemDauVaoTu { get; set; }

        public string? HanDangKyFilter { get; set; }

        public string? SortOrder { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public int TotalItems { get; set; }

        public int TotalPages
        {
            get
            {
                if (TotalItems == 0)
                {
                    return 1;
                }

                return (int)Math.Ceiling(
                    (double)TotalItems / PageSize
                );
            }
        }
    }
}