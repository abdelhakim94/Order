using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._16AddUserBioAndSectionOrder
{
    public partial class _16AddUserBioAndSectionsOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bio",
                schema: "order_schema",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ordre",
                schema: "order_schema",
                table: "menu_section",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ordre",
                schema: "order_schema",
                table: "dish_section",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ordre",
                schema: "order_schema",
                table: "card_section",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bio",
                schema: "order_schema",
                table: "user");

            migrationBuilder.DropColumn(
                name: "ordre",
                schema: "order_schema",
                table: "menu_section");

            migrationBuilder.DropColumn(
                name: "ordre",
                schema: "order_schema",
                table: "dish_section");

            migrationBuilder.DropColumn(
                name: "ordre",
                schema: "order_schema",
                table: "card_section");
        }
    }
}
