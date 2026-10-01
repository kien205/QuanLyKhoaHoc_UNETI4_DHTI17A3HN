// Họ và tên: ...
// Mã sinh viên: ...
// Nội dung thực hiện: Dữ liệu mẫu cho toàn hệ thống.

using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data
{
    public static class DuLieuMau
    {
        private static readonly string[] TenHocVien =
        {
            "Nguyễn Văn An", "Trần Thị Bình", "Lê Hoàng Cường", "Phạm Thị Dung", "Vũ Minh Đức",
            "Đặng Thu Hà", "Bùi Quang Huy", "Hoàng Thị Lan", "Ngô Văn Long", "Đỗ Thị Mai",
            "Dương Quốc Nam", "Lý Thị Oanh", "Trịnh Văn Phúc", "Phan Thị Quỳnh", "Vương Đình Sơn",
            "Hồ Thị Thảo", "Lương Văn Tuấn", "Mai Thị Uyên", "Đinh Công Việt", "Nguyễn Thị Xuân",
            "Trần Văn Yên", "Lê Thị Ánh", "Phạm Quang Bách", "Vũ Thị Chi", "Đặng Văn Dũng",
            "Bùi Thị Giang", "Hoàng Văn Hải", "Ngô Thị Hương", "Đỗ Văn Khánh", "Dương Thị Linh"
        };

        public static void KhoiTao(AppDbContext db)
        {
            db.Database.Migrate();      
            TaoTaiKhoan(db);
            TaoChuongTrinhDaoTao(db);
            TaoHocVien(db);
            TaoLopHoc(db);
            TaoHoSoLichKetQua(db);
        }

        private static void TaoTaiKhoan(AppDbContext db)
        {
            if (db.TaiKhoans.Any()) return;

            var ds = new List<TaiKhoan>
            {
                new() { TenDangNhap = "admin",     MatKhau = "Admin@123", HoTen = "Quản trị viên",      Email = "admin@uneti.edu.vn",     VaiTro = VaiTroTaiKhoan.Admin },
                new() { TenDangNhap = "admin02",   MatKhau = "Admin@123", HoTen = "Quản trị viên phụ",  Email = "admin02@uneti.edu.vn",   VaiTro = VaiTroTaiKhoan.Admin },
                new() { TenDangNhap = "nhanvien1", MatKhau = "Nv@12345",  HoTen = "Nguyễn Thị Lan",     Email = "lan@uneti.edu.vn",       VaiTro = VaiTroTaiKhoan.NhanVienDaoTao },
                new() { TenDangNhap = "nhanvien2", MatKhau = "Nv@12345",  HoTen = "Trần Văn Minh",      Email = "minh@uneti.edu.vn",      VaiTro = VaiTroTaiKhoan.NhanVienDaoTao },
                new() { TenDangNhap = "nhanvien3", MatKhau = "Nv@12345",  HoTen = "Lê Thu Trang",       Email = "trang@uneti.edu.vn",     VaiTro = VaiTroTaiKhoan.NhanVienDaoTao },
                new() { TenDangNhap = "khoa_test", MatKhau = "Hv@12345",  HoTen = "Tài khoản bị khóa",  Email = "khoa@uneti.edu.vn",      VaiTro = VaiTroTaiKhoan.HocVien, TrangThai = false }
            };

            for (int i = 1; i <= 30; i++)
            {
                ds.Add(new TaiKhoan
                {
                    TenDangNhap = $"hocvien{i:00}",
                    MatKhau = "Hv@12345",
                    HoTen = TenHocVien[i - 1],
                    Email = $"hocvien{i:00}@gmail.com",
                    VaiTro = VaiTroTaiKhoan.HocVien
                });
            }

            db.TaiKhoans.AddRange(ds);
            db.SaveChanges();
        }

        private static void TaoChuongTrinhDaoTao(AppDbContext db)
        {
            if (db.ChuongTrinhDaoTaos.Any()) return;

            db.ChuongTrinhDaoTaos.AddRange(
                new ChuongTrinhDaoTao { TenChuongTrinhDaoTao = "Tiếng Anh giao tiếp", MoTa = "Rèn luyện kỹ năng nghe nói trong giao tiếp hằng ngày và công việc.", EmailLienHe = "tienganh@uneti.edu.vn" },
                new ChuongTrinhDaoTao { TenChuongTrinhDaoTao = "Luyện thi IELTS", MoTa = "Lộ trình luyện thi IELTS từ nền tảng đến nâng cao.", EmailLienHe = "ielts@uneti.edu.vn" },
                new ChuongTrinhDaoTao { TenChuongTrinhDaoTao = "Tiếng Nhật", MoTa = "Đào tạo tiếng Nhật từ sơ cấp đến trung cấp theo chuẩn JLPT.", EmailLienHe = "tiengnhat@uneti.edu.vn" },
                new ChuongTrinhDaoTao { TenChuongTrinhDaoTao = "Tin học văn phòng", MoTa = "Word, Excel, PowerPoint ứng dụng trong công việc văn phòng.", EmailLienHe = "tinhoc@uneti.edu.vn" },
                new ChuongTrinhDaoTao { TenChuongTrinhDaoTao = "Kỹ năng mềm", MoTa = "Chương trình đang tạm ngừng triển khai.", EmailLienHe = null, TrangThai = false }
            );
            db.SaveChanges();
        }

        private static void TaoHocVien(AppDbContext db)
        {
            if (db.HocViens.Any()) return;

            var tks = db.TaiKhoans
                .Where(t => t.TenDangNhap.StartsWith("hocvien"))
                .OrderBy(t => t.TenDangNhap).ToList();

            var trinhDo = new[] { "Cơ bản", "Trung cấp", "Nâng cao" };
            var ngheNghiep = new[] { "Sinh viên", "Nhân viên văn phòng", "Kỹ thuật viên", "Kế toán", "Kinh doanh tự do" };
            var diaChi = new[] { "Cầu Giấy, Hà Nội", "Nam Từ Liêm, Hà Nội", "Hà Đông, Hà Nội", "Thanh Xuân, Hà Nội", "Long Biên, Hà Nội", "Đống Đa, Hà Nội" };
            var mucTieu = new[] { "Giao tiếp tự tin trong công việc", "Lấy chứng chỉ để xin việc", "Du học hoặc làm việc nước ngoài", "Nâng cao năng lực chuyên môn" };

            for (int i = 1; i <= tks.Count; i++)
            {
                db.HocViens.Add(new HocVien
                {
                    MaTaiKhoan = tks[i - 1].MaTaiKhoan,
                    HoTen = tks[i - 1].HoTen,
                    NgaySinh = new DateTime(1995 + i % 10, 1 + i % 12, 1 + i % 28),
                    GioiTinh = (i - 1) % 2 == 0 ? "Nam" : "Nữ",
                    SoDienThoai = "0900000" + i.ToString("000"),
                    Email = tks[i - 1].Email,
                    DiaChi = diaChi[i % diaChi.Length],
                    TrinhDoHienTai = trinhDo[i % trinhDo.Length],
                    NgheNghiep = ngheNghiep[i % ngheNghiep.Length],
                    SoThangDaHoc = (i * 3) % 18,
                    MucTieuHoc = mucTieu[i % mucTieu.Length]
                });
            }
            db.SaveChanges();
        }

        private static void TaoLopHoc(AppDbContext db)
        {
            if (db.LopHocs.Any()) return;

            var ct = db.ChuongTrinhDaoTaos.OrderBy(c => c.MaChuongTrinhDaoTao).ToList();
            var hn = DateTime.Today;

  
            LopHoc L(string ten, int ctIndex, int siSo, string trinhDo, double diem, int bd, int han, string trangThai) => new()
            {
                TenLop = ten,
                MaChuongTrinhDaoTao = ct[ctIndex].MaChuongTrinhDaoTao,
                SiSoToiDa = siSo,
                TrinhDoDauVao = trinhDo,
                DiemDauVao = diem,
                NgayBatDauDangKy = hn.AddDays(bd),
                HanDangKy = hn.AddDays(han),
                MoTaLopHoc = $"Lớp {ten} dành cho học viên trình độ {trinhDo.ToLower()}.",
                YeuCauHocVien = diem > 0 ? $"Đạt tối thiểu {diem} điểm kiểm tra đầu vào." : "Không yêu cầu điểm đầu vào.",
                TrangThai = trangThai
            };

            db.LopHocs.AddRange(
                
                L("Tiếng Anh giao tiếp K01", 0, 30, "Cơ bản", 0, -30, 20, TrangThaiLop.DangMoDangKy),
                L("Tiếng Anh giao tiếp K02", 0, 25, "Trung cấp", 40, -10, 30, TrangThaiLop.DangMoDangKy),
                L("Tiếng Anh giao tiếp K03", 0, 20, "Nâng cao", 60, -5, 15, TrangThaiLop.DangMoDangKy),
                L("Tiếng Anh giao tiếp K00", 0, 30, "Cơ bản", 0, -90, -60, TrangThaiLop.DaDong),
                
                L("IELTS 5.0", 1, 20, "Trung cấp", 50, -20, 25, TrangThaiLop.DangMoDangKy),
                L("IELTS 6.5", 1, 15, "Nâng cao", 65, -15, 10, TrangThaiLop.DangMoDangKy),
                L("IELTS Cấp tốc", 1, 3, "Nâng cao", 70, -60, -10, TrangThaiLop.DangMoDangKy),
                L("IELTS Foundation", 1, 25, "Cơ bản", 0, 10, 40, TrangThaiLop.ChuaMo),
               
                L("Tiếng Nhật N5", 2, 25, "Cơ bản", 0, -12, 18, TrangThaiLop.DangMoDangKy),
                L("Tiếng Nhật N4", 2, 20, "Trung cấp", 45, -8, 22, TrangThaiLop.DangMoDangKy),
                L("Tiếng Nhật N3", 2, 15, "Nâng cao", 60, -50, -20, TrangThaiLop.DaDong),
                L("Tiếng Nhật giao tiếp", 2, 20, "Cơ bản", 0, -5, 12, TrangThaiLop.TamDung),
               
                L("Tin học văn phòng cơ bản", 3, 40, "Cơ bản", 0, -25, 30, TrangThaiLop.DangMoDangKy),
                L("Excel nâng cao", 3, 30, "Trung cấp", 50, -18, 14, TrangThaiLop.DangMoDangKy),
                L("Tin học văn phòng K00", 3, 35, "Cơ bản", 0, -100, -70, TrangThaiLop.DaDong)
            );
            db.SaveChanges();
        }

      
        private static void TaoHoSoLichKetQua(AppDbContext db)
        {
            if (db.HoSoDangKyHocs.Any()) return;

            var hv = db.HocViens.OrderBy(h => h.MaHocVien).ToList();
            var lop = db.LopHocs.OrderBy(l => l.MaLop).ToList();
            if (hv.Count < 30 || lop.Count < 15) return;

            var hn = DateTime.Today;

          
            var ke = new (int h, int l, string tt)[]
            {
               
                (1, 4,  TrangThaiHoSo.DuocXepLop),  (2, 4,  TrangThaiHoSo.DuocXepLop),
                (3, 7,  TrangThaiHoSo.DuocXepLop),  (4, 7,  TrangThaiHoSo.DuocXepLop),
                (5, 7,  TrangThaiHoSo.DuocXepLop),  (6, 13, TrangThaiHoSo.DuocXepLop),
               
                (7, 11, TrangThaiHoSo.ChuaDuocXepLop), (8, 13, TrangThaiHoSo.ChuaDuocXepLop), (9, 15, TrangThaiHoSo.ChuaDuocXepLop),
                
                (10, 1, TrangThaiHoSo.DaKiemTraDauVao), (11, 5, TrangThaiHoSo.DaKiemTraDauVao),
                
                (12, 2, TrangThaiHoSo.ChoKiemTraDauVao), (13, 9, TrangThaiHoSo.ChoKiemTraDauVao),
                (14, 13, TrangThaiHoSo.ChoKiemTraDauVao), (15, 14, TrangThaiHoSo.ChoKiemTraDauVao),
                
                (16, 1, TrangThaiHoSo.DuDieuKien), (17, 3, TrangThaiHoSo.DuDieuKien), (18, 5, TrangThaiHoSo.DuDieuKien),
                (19, 10, TrangThaiHoSo.DuDieuKien), (20, 13, TrangThaiHoSo.DuDieuKien), (21, 14, TrangThaiHoSo.DuDieuKien),
               
                (22, 2, TrangThaiHoSo.KhongDuDieuKien), (23, 6, TrangThaiHoSo.KhongDuDieuKien),
                (24, 9, TrangThaiHoSo.KhongDuDieuKien), (25, 14, TrangThaiHoSo.KhongDuDieuKien),
               
                (26, 1, TrangThaiHoSo.ChoDuyet), (27, 1, TrangThaiHoSo.ChoDuyet), (28, 2, TrangThaiHoSo.ChoDuyet),
                (29, 3, TrangThaiHoSo.ChoDuyet), (30, 5, TrangThaiHoSo.ChoDuyet), (1, 6, TrangThaiHoSo.ChoDuyet),
                (2, 9, TrangThaiHoSo.ChoDuyet),  (3, 10, TrangThaiHoSo.ChoDuyet), (4, 13, TrangThaiHoSo.ChoDuyet),
                (5, 14, TrangThaiHoSo.ChoDuyet), (6, 2, TrangThaiHoSo.ChoDuyet),  (7, 1, TrangThaiHoSo.ChoDuyet),
                
                (8, 3, TrangThaiHoSo.DaHuy), (9, 5, TrangThaiHoSo.DaHuy), (10, 6, TrangThaiHoSo.DaHuy), (11, 9, TrangThaiHoSo.DaHuy)
            };

            var lyDo = new[]
            {
                "Muốn nâng cao kỹ năng phục vụ công việc hiện tại.",
                "Cần chứng chỉ để xét tuyển hoặc thăng tiến.",
                "Chuẩn bị cho kế hoạch học tập và làm việc sắp tới.",
                "Có hứng thú và muốn học bài bản từ giảng viên."
            };
            var giaoVien = new[] { "ThS. Nguyễn Văn Hùng", "ThS. Lê Thị Mai", "TS. Phạm Quốc Bảo", "ThS. Trần Thu Hà" };
            var gioBatDau = new[] { 8, 10, 14, 16 };

            int n = 0, soLichQuaKhu = 0, soLichTuongLai = 0;

            foreach (var (h, l, tt) in ke)
            {
                n++;
                var hocVien = hv[h - 1];
                var lopHoc = lop[l - 1];

                
                int gioiHan = tt switch
                {
                    TrangThaiHoSo.ChoDuyet => 1,
                    TrangThaiHoSo.ChoKiemTraDauVao => 4,
                    TrangThaiHoSo.DaKiemTraDauVao or TrangThaiHoSo.DuocXepLop or TrangThaiHoSo.ChuaDuocXepLop => 8,
                    _ => 3
                };
                var ngayNop = lopHoc.NgayBatDauDangKy.AddDays(1 + n % 3);
                var tran = hn.AddDays(-gioiHan);
                if (ngayNop > tran) ngayNop = tran;
                ngayNop = ngayNop.AddHours(8 + n % 9);

                var hoSo = new HoSoDangKyHoc
                {
                    MaHocVien = hocVien.MaHocVien,
                    MaLop = lopHoc.MaLop,
                    NgayNop = ngayNop,
                    LyDoDangKy = lyDo[n % lyDo.Length],
                    TrangThai = tt
                };

                if (tt == TrangThaiHoSo.KhongDuDieuKien)
                {
                    hoSo.NgayXuLy = ngayNop.Date.AddDays(1).AddHours(9);
                    hoSo.NhanXetXetDuyet = "Chưa đáp ứng yêu cầu đầu vào của lớp học.";
                }
                else if (tt == TrangThaiHoSo.DaHuy)
                {
                    hoSo.NgayXuLy = ngayNop.Date.AddDays(1).AddHours(9);
                    hoSo.GhiChu = "Học viên tự hủy hồ sơ.";
                }
                else if (tt != TrangThaiHoSo.ChoDuyet)
                {
                    hoSo.NgayXuLy = ngayNop.Date.AddDays(1).AddHours(9);
                    hoSo.NhanXetXetDuyet = "Hồ sơ đáp ứng yêu cầu của lớp học.";
                }
                db.HoSoDangKyHocs.Add(hoSo);

                bool lichDaHoanThanh = tt is TrangThaiHoSo.DaKiemTraDauVao or TrangThaiHoSo.DuocXepLop or TrangThaiHoSo.ChuaDuocXepLop;
                bool lichDaLen = tt == TrangThaiHoSo.ChoKiemTraDauVao;
                bool lichDaHuy = tt == TrangThaiHoSo.DaHuy && h == 11 && l == 9;

                if (lichDaHoanThanh || lichDaLen || lichDaHuy)
                {
                    int slot;
                    DateTime ngayLich;
                    if (lichDaHoanThanh) { slot = soLichQuaKhu++; ngayLich = hn.AddDays(-2 - slot / 4); }
                    else { slot = soLichTuongLai++; ngayLich = hn.AddDays(2 + slot / 4); }

                    var batDau = ngayLich.AddHours(gioBatDau[slot % 4]);   
                    bool trucTiep = slot % 2 == 0;

                    var lich = new LichKiemTraDauVao
                    {
                        HoSoDangKyHoc = hoSo,
                        ThoiGianBatDau = batDau,
                        ThoiGianKetThuc = batDau.AddHours(1),
                        HinhThucKiemTra = trucTiep ? "Trực tiếp" : "Trực tuyến",
                        DiaDiemHoacLienKet = trucTiep ? $"Phòng A{101 + slot % 5}" : "https://meet.google.com/abc-defg-hij",
                        GiaoVienKiemTra = giaoVien[slot % giaoVien.Length],
                        GhiChu = "Học viên mang theo giấy tờ tùy thân.",
                        TrangThai = lichDaHoanThanh ? TrangThaiLich.DaHoanThanh
                                  : lichDaHuy ? TrangThaiLich.DaHuy
                                  : TrangThaiLich.DaLenLich
                    };
                    db.LichKiemTraDauVaos.Add(lich);

                    
                    if (tt == TrangThaiHoSo.DuocXepLop || tt == TrangThaiHoSo.ChuaDuocXepLop)
                    {
                        bool duoc = tt == TrangThaiHoSo.DuocXepLop;
                        db.KetQuaXepLops.Add(new KetQuaXepLop
                        {
                            HoSoDangKyHoc = hoSo,
                            DiemDanhGia = duoc ? 72 + (n * 5) % 25 : 30 + (n * 3) % 15,
                            NhanXet = duoc ? "Đạt yêu cầu, xếp vào lớp đã đăng ký." : "Chưa đạt yêu cầu đầu vào, cần học lớp phù hợp hơn.",
                            KetQua = duoc ? TrangThaiHoSo.DuocXepLop : TrangThaiHoSo.ChuaDuocXepLop,
                            NgayCapNhat = lich.ThoiGianKetThuc.AddDays(1)
                        });
                    }
                }
            }
            db.SaveChanges();
        }
    }
}