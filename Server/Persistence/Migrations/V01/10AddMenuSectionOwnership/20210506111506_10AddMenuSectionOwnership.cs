using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._10AddMenuSectionOwnership
{
    public partial class _10AddMenuSectionOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "menu_owns",
                schema: "order_schema",
                table: "menu_section",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "menu_owns",
                schema: "order_schema",
                table: "menu_section");
        }
    }
}
