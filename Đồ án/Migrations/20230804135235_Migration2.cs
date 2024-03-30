using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Đồ_án.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
	        name: "soNguoi",
	        table: "loaiPhong",
	        nullable: false);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
