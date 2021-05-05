using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._08AddLongitudeAndLatitudeToCity
{
    public partial class _08AddLongitudeAndLatitudeToCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMandatory",
                schema: "order_schema",
                table: "menu_dish",
                newName: "is_mandatory");

            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                schema: "order_schema",
                table: "city",
                type: "decimal(22, 20)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                schema: "order_schema",
                table: "city",
                type: "decimal(23, 20)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropColumn(
                name: "longitude",
                schema: "order_schema",
                table: "city");

            migrationBuilder.RenameColumn(
                name: "is_mandatory",
                schema: "order_schema",
                table: "menu_dish",
                newName: "IsMandatory");
        }
    }
}
