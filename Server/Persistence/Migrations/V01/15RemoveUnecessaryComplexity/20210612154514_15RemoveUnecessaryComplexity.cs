using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._15RemoveUnecessaryComplexity
{
    public partial class _15RemoveUnecessaryComplexity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_dish",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "card_menu",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu_dish",
                schema: "order_schema");

            migrationBuilder.DropColumn(
                name: "is_mandatory",
                schema: "order_schema",
                table: "dish_section");

            migrationBuilder.DropColumn(
                name: "is_menu_only",
                schema: "order_schema",
                table: "dish");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_mandatory",
                schema: "order_schema",
                table: "dish_section",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_menu_only",
                schema: "order_schema",
                table: "dish",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "card_dish",
                schema: "order_schema",
                columns: table => new
                {
                    id_dish = table.Column<int>(type: "integer", nullable: false),
                    id_card = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARD_DISH", x => new { x.id_dish, x.id_card });
                    table.ForeignKey(
                        name: "FK_CARD_DISH_CARD",
                        column: x => x.id_card,
                        principalSchema: "order_schema",
                        principalTable: "card",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CARD_DISH_DISH",
                        column: x => x.id_dish,
                        principalSchema: "order_schema",
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card_menu",
                schema: "order_schema",
                columns: table => new
                {
                    id_card = table.Column<int>(type: "integer", nullable: false),
                    id_menu = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARD_MENU", x => new { x.id_card, x.id_menu });
                    table.ForeignKey(
                        name: "FK_CARD_MENU_CARD",
                        column: x => x.id_card,
                        principalSchema: "order_schema",
                        principalTable: "card",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CARD_MENU_MENU",
                        column: x => x.id_menu,
                        principalSchema: "order_schema",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_dish",
                schema: "order_schema",
                columns: table => new
                {
                    id_dish = table.Column<int>(type: "integer", nullable: false),
                    id_menu = table.Column<int>(type: "integer", nullable: false),
                    is_mandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_DISH", x => new { x.id_dish, x.id_menu });
                    table.ForeignKey(
                        name: "FK_MENU_DISH_DISH",
                        column: x => x.id_dish,
                        principalSchema: "order_schema",
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MENU_DISH_MENU",
                        column: x => x.id_menu,
                        principalSchema: "order_schema",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_dish_id_card",
                schema: "order_schema",
                table: "card_dish",
                column: "id_card");

            migrationBuilder.CreateIndex(
                name: "IX_card_menu_id_menu",
                schema: "order_schema",
                table: "card_menu",
                column: "id_menu");

            migrationBuilder.CreateIndex(
                name: "IX_menu_dish_id_menu",
                schema: "order_schema",
                table: "menu_dish",
                column: "id_menu");
        }
    }
}
