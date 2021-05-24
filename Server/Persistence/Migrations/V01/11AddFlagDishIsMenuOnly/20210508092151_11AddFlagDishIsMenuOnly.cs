using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._11AddFlagDishIsMenuOnly
{
    public partial class _11AddFlagDishIsMenuOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_menu_only",
                schema: "order_schema",
                table: "dish",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_menu_only",
                schema: "order_schema",
                table: "dish");
        }
    }
}
