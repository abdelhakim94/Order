using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Order.Server.Persistence.Migrations.V01._14AddOptionAndExtraToDishAndMenu
{
    public partial class _14AddOptionAndExtraToDishAndMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "extra",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false),
                    price = table.Column<decimal>(type: "decimal(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXTRA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "option",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPTION", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dish_extra",
                schema: "order_schema",
                columns: table => new
                {
                    id_extra = table.Column<int>(type: "integer", nullable: false),
                    id_dish = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISH_EXTRA", x => new { x.id_dish, x.id_extra });
                    table.ForeignKey(
                        name: "FK_dish_extra_dish_id_dish",
                        column: x => x.id_dish,
                        principalSchema: "order_schema",
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dish_extra_extra_id_extra",
                        column: x => x.id_extra,
                        principalSchema: "order_schema",
                        principalTable: "extra",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_extra",
                schema: "order_schema",
                columns: table => new
                {
                    id_menu = table.Column<int>(type: "integer", nullable: false),
                    id_extra = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_EXTRA", x => new { x.id_menu, x.id_extra });
                    table.ForeignKey(
                        name: "FK_menu_extra_extra_id_extra",
                        column: x => x.id_extra,
                        principalSchema: "order_schema",
                        principalTable: "extra",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menu_extra_menu_id_menu",
                        column: x => x.id_menu,
                        principalSchema: "order_schema",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish_option",
                schema: "order_schema",
                columns: table => new
                {
                    id_option = table.Column<int>(type: "integer", nullable: false),
                    id_dish = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISH_OPTION", x => new { x.id_dish, x.id_option });
                    table.ForeignKey(
                        name: "FK_dish_option_dish_id_dish",
                        column: x => x.id_dish,
                        principalSchema: "order_schema",
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dish_option_option_id_option",
                        column: x => x.id_option,
                        principalSchema: "order_schema",
                        principalTable: "option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_option",
                schema: "order_schema",
                columns: table => new
                {
                    id_menu = table.Column<int>(type: "integer", nullable: false),
                    id_option = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_OPTION", x => new { x.id_menu, x.id_option });
                    table.ForeignKey(
                        name: "FK_menu_option_menu_id_menu",
                        column: x => x.id_menu,
                        principalSchema: "order_schema",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menu_option_option_id_option",
                        column: x => x.id_option,
                        principalSchema: "order_schema",
                        principalTable: "option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dish_extra_id_extra",
                schema: "order_schema",
                table: "dish_extra",
                column: "id_extra");

            migrationBuilder.CreateIndex(
                name: "IX_dish_option_id_option",
                schema: "order_schema",
                table: "dish_option",
                column: "id_option");

            migrationBuilder.CreateIndex(
                name: "IX_menu_extra_id_extra",
                schema: "order_schema",
                table: "menu_extra",
                column: "id_extra");

            migrationBuilder.CreateIndex(
                name: "IX_menu_option_id_option",
                schema: "order_schema",
                table: "menu_option",
                column: "id_option");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dish_extra",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "dish_option",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu_extra",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu_option",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "extra",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "option",
                schema: "order_schema");
        }
    }
}
