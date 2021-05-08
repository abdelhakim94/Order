using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._12AddDefaultValueToAddress2
{
    public partial class _12AddDefaultValueToAddress2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "address2",
                schema: "order_schema",
                table: "user_address",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<string>(
                name: "address2",
                schema: "order_schema",
                table: "address",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "address2",
                schema: "order_schema",
                table: "user_address",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "address2",
                schema: "order_schema",
                table: "address",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldDefaultValue: "");
        }
    }
}
