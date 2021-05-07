using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Order.Server.Persistence.Migrations.V01._06AddDishRelatedTables
{
    public partial class _06AddDishRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "card",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARD", x => x.id);
                    table.ForeignKey(
                        name: "FK_CARD_USER",
                        column: x => x.id_user,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying", maxLength: 200, nullable: true),
                    picture = table.Column<string>(type: "character varying", nullable: true),
                    price = table.Column<decimal>(type: "decimal(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISH", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying", maxLength: 200, nullable: true),
                    picture = table.Column<string>(type: "character varying", nullable: true),
                    price = table.Column<decimal>(type: "decimal(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "section",
                schema: "order_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECTION", x => x.id);
                });

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
                name: "dish_category",
                schema: "order_schema",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "integer", nullable: false),
                    id_dish = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISH_CATEGORY", x => new { x.id_category, x.id_dish });
                    table.ForeignKey(
                        name: "FK_DISH_CATEGORY_CATEGORY",
                        column: x => x.id_category,
                        principalSchema: "order_schema",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DISH_CATEGORY_DISH",
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
                    id_menu = table.Column<int>(type: "integer", nullable: false),
                    id_card = table.Column<int>(type: "integer", nullable: false)
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
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "card_section",
                schema: "order_schema",
                columns: table => new
                {
                    id_section = table.Column<int>(type: "integer", nullable: false),
                    id_card = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARD_SECTION", x => new { x.id_section, x.id_card });
                    table.ForeignKey(
                        name: "FK_CARD_SECTION_CARD",
                        column: x => x.id_card,
                        principalSchema: "order_schema",
                        principalTable: "card",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CARD_SECTION_SECTION",
                        column: x => x.id_section,
                        principalSchema: "order_schema",
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish_section",
                schema: "order_schema",
                columns: table => new
                {
                    id_section = table.Column<int>(type: "integer", nullable: false),
                    id_dish = table.Column<int>(type: "integer", nullable: false),
                    is_mandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISH_SECTION", x => new { x.id_dish, x.id_section });
                    table.ForeignKey(
                        name: "FK_DISH_SECTION_DISH",
                        column: x => x.id_dish,
                        principalSchema: "order_schema",
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DISH_SECTION_SECTION",
                        column: x => x.id_section,
                        principalSchema: "order_schema",
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_section",
                schema: "order_schema",
                columns: table => new
                {
                    id_menu = table.Column<int>(type: "integer", nullable: false),
                    id_section = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_SECTION", x => new { x.id_menu, x.id_section });
                    table.ForeignKey(
                        name: "FK_MENU_SECTION_MENU",
                        column: x => x.id_menu,
                        principalSchema: "order_schema",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MENU_SECTION_SECTION",
                        column: x => x.id_section,
                        principalSchema: "order_schema",
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_id_user",
                schema: "order_schema",
                table: "card",
                column: "id_user");

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
                name: "IX_card_section_id_card",
                schema: "order_schema",
                table: "card_section",
                column: "id_card");

            migrationBuilder.CreateIndex(
                name: "IX_dish_category_id_dish",
                schema: "order_schema",
                table: "dish_category",
                column: "id_dish");

            migrationBuilder.CreateIndex(
                name: "IX_dish_section_id_section",
                schema: "order_schema",
                table: "dish_section",
                column: "id_section");

            migrationBuilder.CreateIndex(
                name: "IX_menu_dish_id_menu",
                schema: "order_schema",
                table: "menu_dish",
                column: "id_menu");

            migrationBuilder.CreateIndex(
                name: "IX_menu_section_id_section",
                schema: "order_schema",
                table: "menu_section",
                column: "id_section");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_dish",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "card_menu",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "card_section",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "dish_category",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "dish_section",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu_dish",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu_section",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "card",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "dish",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "menu",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "section",
                schema: "order_schema");
        }
    }
}
