using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._05AddProfile
{
    public partial class _05AddProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profile",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFILE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                schema: "order_schema",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    id_profile = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_PROFILE", x => new { x.id_user, x.id_profile });
                    table.ForeignKey(
                        name: "FK_USER_PROFILE_PROFILE",
                        column: x => x.id_profile,
                        principalSchema: "order_schema",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_PROFILE_USER",
                        column: x => x.id_user,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_profile_id_profile",
                schema: "order_schema",
                table: "user_profile",
                column: "id_profile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_profile",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "order_schema");
        }
    }
}
