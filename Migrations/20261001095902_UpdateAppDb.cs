using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.RenameColumn(
                name: "chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                newName: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao");

            migrationBuilder.RenameIndex(
                name: "IX_LopHocs_chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                newName: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao");

            migrationBuilder.AddColumn<int>(
                name: "MaChuongTrinhDaoTao",
                table: "LopHocs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                principalTable: "ChuongTrinhDaoTaos",
                principalColumn: "MaChuongTrinhDaoTao",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.DropColumn(
                name: "MaChuongTrinhDaoTao",
                table: "LopHocs");

            migrationBuilder.RenameColumn(
                name: "ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                newName: "chuongTrinhDaoTaoMaChuongTrinhDaoTao");

            migrationBuilder.RenameIndex(
                name: "IX_LopHocs_ChuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                newName: "IX_LopHocs_chuongTrinhDaoTaoMaChuongTrinhDaoTao");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocs_ChuongTrinhDaoTaos_chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                table: "LopHocs",
                column: "chuongTrinhDaoTaoMaChuongTrinhDaoTao",
                principalTable: "ChuongTrinhDaoTaos",
                principalColumn: "MaChuongTrinhDaoTao",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
