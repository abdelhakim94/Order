using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Model.Migrations.V01.AddFisrtAndLastNameToUser
{
    public partial class AddFirstAndLastNameToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "order_schema",
                table: "user",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "order_schema",
                table: "user",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "order_schema",
                table: "user");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "order_schema",
                table: "user");
        }
    }
}
