using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class AppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuongTrinhDaoTaos",
                columns: table => new
                {
                    MaChuongTrinhDaoTao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuongTrinhDaoTao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EmailLienHe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuongTrinhDaoTaos", x => x.MaChuongTrinhDaoTao);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "LopHocs",
                columns: table => new
                {
                    MaLop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    chuongTrinhDaoTaoMaChuongTrinhDaoTao = table.Column<int>(type: "int", nullable: true),
                    SiSoToiDa = table.Column<int>(type: "int", nullable: false),
                    TrinhDoDauVao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiemDauVao = table.Column<double>(type: "float", nullable: false),
                    NgayBatDauDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoTaLopHoc = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    YeuCauHocVien = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHocs", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_LopHocs_ChuongTrinhDaoTaos_chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                        column: x => x.chuongTrinhDaoTaoMaChuongTrinhDaoTao,
                        principalTable: "ChuongTrinhDaoTaos",
                        principalColumn: "MaChuongTrinhDaoTao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HocViens",
                columns: table => new
                {
                    MaHocVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrinhDoHienTai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgheNghiep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoThangDaHoc = table.Column<int>(type: "int", nullable: false),
                    MucTieuHoc = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocViens", x => x.MaHocVien);
                    table.ForeignKey(
                        name: "FK_HocViens_TaiKhoans_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoSoDangKyHocs",
                columns: table => new
                {
                    MaHoSo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHocVien = table.Column<int>(type: "int", nullable: false),
                    HocVienMaHocVien = table.Column<int>(type: "int", nullable: true),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    LopHocMaLop = table.Column<int>(type: "int", nullable: true),
                    NgayNop = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDoDangKy = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NgayXuLy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NhanXetXetDuyet = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoDangKyHocs", x => x.MaHoSo);
                    table.ForeignKey(
                        name: "FK_HoSoDangKyHocs_HocViens_HocVienMaHocVien",
                        column: x => x.HocVienMaHocVien,
                        principalTable: "HocViens",
                        principalColumn: "MaHocVien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoSoDangKyHocs_LopHocs_LopHocMaLop",
                        column: x => x.LopHocMaLop,
                        principalTable: "LopHocs",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaXepLops",
                columns: table => new
                {
                    MaKetQuaXepLop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    DiemDanhGia = table.Column<double>(type: "float", nullable: true),
                    NhanXet = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    KetQua = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaXepLops", x => x.MaKetQuaXepLop);
                    table.ForeignKey(
                        name: "FK_KetQuaXepLops_HoSoDangKyHocs_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoDangKyHocs",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichKiemTraDauVaos",
                columns: table => new
                {
                    MaLichKiemTraDauVao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    HoSoDangKyHocMaHoSo = table.Column<int>(type: "int", nullable: true),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HinhThucKiemTra = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaDiemHoacLienKet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GiaoVienKiemTra = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichKiemTraDauVaos", x => x.MaLichKiemTraDauVao);
                    table.ForeignKey(
                        name: "FK_LichKiemTraDauVaos_HoSoDangKyHocs_HoSoDangKyHocMaHoSo",
                        column: x => x.HoSoDangKyHocMaHoSo,
                        principalTable: "HoSoDangKyHocs",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuongTrinhDaoTaos_TenChuongTrinhDaoTao",
                table: "ChuongTrinhDaoTaos",
                column: "TenChuongTrinhDaoTao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HocViens_MaTaiKhoan",
                table: "HocViens",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_HocVienMaHocVien",
                table: "HoSoDangKyHocs",
                column: "HocVienMaHocVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_LopHocMaLop",
                table: "HoSoDangKyHocs",
                column: "LopHocMaLop");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaXepLops_MaHoSo",
                table: "KetQuaXepLops",
                column: "MaHoSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichKiemTraDauVaos_HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos",
                column: "HoSoDangKyHocMaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocs_chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "chuongTrinhDaoTaoMaChuongTrinhDaoTao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KetQuaXepLops");

            migrationBuilder.DropTable(
                name: "LichKiemTraDauVaos");

            migrationBuilder.DropTable(
                name: "HoSoDangKyHocs");

            migrationBuilder.DropTable(
                name: "HocViens");

            migrationBuilder.DropTable(
                name: "LopHocs");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "ChuongTrinhDaoTaos");
        }
    }
}
