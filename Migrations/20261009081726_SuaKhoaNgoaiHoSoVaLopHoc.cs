using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class SuaKhoaNgoaiHoSoVaLopHoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoSoDangKyHocs_HocViens_HocVienMaHocVien",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoSoDangKyHocs_LopHocs_LopHocMaLop",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_HoSoDangKyHocs_HocVienMaHocVien",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropIndex(
                name: "IX_HoSoDangKyHocs_LopHocMaLop",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropColumn(
                name: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropColumn(
                name: "HocVienMaHocVien",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropColumn(
                name: "LopHocMaLop",
                table: "HoSoDangKyHocs");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocs_MaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "MaChuongTrinhDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_MaHocVien",
                table: "HoSoDangKyHocs",
                column: "MaHocVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_MaLop",
                table: "HoSoDangKyHocs",
                column: "MaLop");

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoDangKyHocs_HocViens_MaHocVien",
                table: "HoSoDangKyHocs",
                column: "MaHocVien",
                principalTable: "HocViens",
                principalColumn: "MaHocVien",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoDangKyHocs_LopHocs_MaLop",
                table: "HoSoDangKyHocs",
                column: "MaLop",
                principalTable: "LopHocs",
                principalColumn: "MaLop",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_MaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "MaChuongTrinhDaoTao",
                principalTable: "ChuongTrinhDaoTaos",
                principalColumn: "MaChuongTrinhDaoTao",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoSoDangKyHocs_HocViens_MaHocVien",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoSoDangKyHocs_LopHocs_MaLop",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_MaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_LopHocs_MaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_HoSoDangKyHocs_MaHocVien",
                table: "HoSoDangKyHocs");

            migrationBuilder.DropIndex(
                name: "IX_HoSoDangKyHocs_MaLop",
                table: "HoSoDangKyHocs");

            migrationBuilder.AddColumn<int>(
                name: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HocVienMaHocVien",
                table: "HoSoDangKyHocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LopHocMaLop",
                table: "HoSoDangKyHocs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_HocVienMaHocVien",
                table: "HoSoDangKyHocs",
                column: "HocVienMaHocVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDangKyHocs_LopHocMaLop",
                table: "HoSoDangKyHocs",
                column: "LopHocMaLop");

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoDangKyHocs_HocViens_HocVienMaHocVien",
                table: "HoSoDangKyHocs",
                column: "HocVienMaHocVien",
                principalTable: "HocViens",
                principalColumn: "MaHocVien",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoDangKyHocs_LopHocs_LopHocMaLop",
                table: "HoSoDangKyHocs",
                column: "LopHocMaLop",
                principalTable: "LopHocs",
                principalColumn: "MaLop",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                principalTable: "ChuongTrinhDaoTaos",
                principalColumn: "MaChuongTrinhDaoTao",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
