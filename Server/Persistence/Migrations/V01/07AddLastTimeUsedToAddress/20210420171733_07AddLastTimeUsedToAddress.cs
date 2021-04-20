using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._07AddLastTimeUsedToAddress
{
    public partial class _07AddLastTimeUsedToAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "last_time_used",
                schema: "order_schema",
                table: "user_address",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_time_used",
                schema: "order_schema",
                table: "user_address");
        }
    }
}
