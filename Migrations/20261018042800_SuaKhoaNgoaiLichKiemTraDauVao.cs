using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class SuaKhoaNgoaiLichKiemTraDauVao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichKiemTraDauVaos_HoSoDangKyHocs_HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos");

            migrationBuilder.DropIndex(
                name: "IX_LichKiemTraDauVaos_HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos");

            migrationBuilder.DropColumn(
                name: "HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos");

            migrationBuilder.CreateIndex(
                name: "IX_LichKiemTraDauVaos_MaHoSo",
                table: "LichKiemTraDauVaos",
                column: "MaHoSo");

            migrationBuilder.AddForeignKey(
                name: "FK_LichKiemTraDauVaos_HoSoDangKyHocs_MaHoSo",
                table: "LichKiemTraDauVaos",
                column: "MaHoSo",
                principalTable: "HoSoDangKyHocs",
                principalColumn: "MaHoSo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichKiemTraDauVaos_HoSoDangKyHocs_MaHoSo",
                table: "LichKiemTraDauVaos");

            migrationBuilder.DropIndex(
                name: "IX_LichKiemTraDauVaos_MaHoSo",
                table: "LichKiemTraDauVaos");

            migrationBuilder.AddColumn<int>(
                name: "HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichKiemTraDauVaos_HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos",
                column: "HoSoDangKyHocMaHoSo");

            migrationBuilder.AddForeignKey(
                name: "FK_LichKiemTraDauVaos_HoSoDangKyHocs_HoSoDangKyHocMaHoSo",
                table: "LichKiemTraDauVaos",
                column: "HoSoDangKyHocMaHoSo",
                principalTable: "HoSoDangKyHocs",
                principalColumn: "MaHoSo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
