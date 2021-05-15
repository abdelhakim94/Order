using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._13AddPictureToUser
{
    public partial class _13AddPictureToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                schema: "order_schema",
                table: "user",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                schema: "order_schema",
                table: "user");
        }
    }
}
