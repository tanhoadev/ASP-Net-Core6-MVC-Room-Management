using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Đồ_án.Migrations
{
    /// <inheritdoc />
    public partial class Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "anhDaiDien",
                table: "nguoiThue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "matSauCMND",
                table: "nguoiThue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "matTruocCCCD",
                table: "nguoiThue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "trangThai",
                table: "hoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_nguoiThue_CMND_CCCD",
                table: "nguoiThue",
                column: "CMND_CCCD");

            migrationBuilder.CreateTable(
                name: "taikhoanNguoiDung",
                columns: table => new
                {
                    CMND_CCCD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    matKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taikhoanNguoiDung", x => x.CMND_CCCD);
                    table.ForeignKey(
                        name: "FK_TaiKhoanNguoiDung_NguoiThue",
                        column: x => x.CMND_CCCD,
                        principalTable: "nguoiThue",
                        principalColumn: "CMND_CCCD",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "taikhoanNguoiDung");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_nguoiThue_CMND_CCCD",
                table: "nguoiThue");

            migrationBuilder.DropColumn(
                name: "anhDaiDien",
                table: "nguoiThue");

            migrationBuilder.DropColumn(
                name: "matSauCMND",
                table: "nguoiThue");

            migrationBuilder.DropColumn(
                name: "matTruocCCCD",
                table: "nguoiThue");

            migrationBuilder.DropColumn(
                name: "trangThai",
                table: "hoaDon");
        }
    }
}
