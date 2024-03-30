using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Đồ_án.Migrations
{
    /// <inheritdoc />
    public partial class Migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoanNguoiDung_NguoiThue",
                table: "taikhoanNguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_taikhoanNguoiDung",
                table: "taikhoanNguoiDung");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_nguoiThue_CMND_CCCD",
                table: "nguoiThue");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "taikhoanNguoiDung",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_taikhoanNguoiDung",
                table: "taikhoanNguoiDung",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_taikhoanNguoiDung",
                table: "taikhoanNguoiDung");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "taikhoanNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_taikhoanNguoiDung",
                table: "taikhoanNguoiDung",
                column: "CMND_CCCD");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_nguoiThue_CMND_CCCD",
                table: "nguoiThue",
                column: "CMND_CCCD");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoanNguoiDung_NguoiThue",
                table: "taikhoanNguoiDung",
                column: "CMND_CCCD",
                principalTable: "nguoiThue",
                principalColumn: "CMND_CCCD",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
