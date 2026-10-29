using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
