using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class SuaKhoaNgoaiLopHocVaChuongTrinhDaoTaodotnet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropColumn(
                name: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocs_MaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "MaChuongTrinhDaoTao");

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
                name: "FK_LopHocs_ChuongTrinhDaoTaos_MaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropIndex(
                name: "IX_LopHocs_MaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.AddColumn<int>(
                name: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao");

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
