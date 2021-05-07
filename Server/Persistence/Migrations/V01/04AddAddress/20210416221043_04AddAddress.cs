using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Server.Persistence.Migrations.V01._04AddAddress
{
    public partial class _04AddAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wilaya",
                schema: "order_schema",
                columns: table => new
                {
                    code = table.Column<string>(type: "character varying", maxLength: 2, nullable: false),
                    zip_code = table.Column<string>(type: "character varying", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WILAYA", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "city",
                schema: "order_schema",
                columns: table => new
                {
                    zip_code = table.Column<string>(type: "character varying", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "character varying", maxLength: 30, nullable: false),
                    code_wilaya = table.Column<string>(type: "character varying", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITY", x => x.zip_code);
                    table.ForeignKey(
                        name: "FK_CITY_WILAYA",
                        column: x => x.code_wilaya,
                        principalSchema: "order_schema",
                        principalTable: "wilaya",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                schema: "order_schema",
                columns: table => new
                {
                    address1 = table.Column<string>(type: "character varying", nullable: false),
                    address2 = table.Column<string>(type: "character varying", nullable: false),
                    zip_code_city = table.Column<string>(type: "character varying", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADDRESS", x => new { x.address1, x.address2, x.zip_code_city });
                    table.ForeignKey(
                        name: "FK_ADDRESS_CITY",
                        column: x => x.zip_code_city,
                        principalSchema: "order_schema",
                        principalTable: "city",
                        principalColumn: "zip_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_address",
                schema: "order_schema",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    address1 = table.Column<string>(type: "character varying", nullable: false),
                    address2 = table.Column<string>(type: "character varying", nullable: false),
                    zip_code_city = table.Column<string>(type: "character varying", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ADDRESS", x => new { x.id_user, x.address1, x.address2, x.zip_code_city });
                    table.ForeignKey(
                        name: "FK_USER_ADDRESS_ADDRESS",
                        columns: x => new { x.address1, x.address2, x.zip_code_city },
                        principalSchema: "order_schema",
                        principalTable: "address",
                        principalColumns: new[] { "address1", "address2", "zip_code_city" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_ADDRESS_USER",
                        column: x => x.id_user,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_zip_code_city",
                schema: "order_schema",
                table: "address",
                column: "zip_code_city");

            migrationBuilder.CreateIndex(
                name: "INDEX_NAME_CITY",
                schema: "order_schema",
                table: "city",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_city_code_wilaya",
                schema: "order_schema",
                table: "city",
                column: "code_wilaya");

            migrationBuilder.CreateIndex(
                name: "IX_user_address_address1_address2_zip_code_city",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "address1", "address2", "zip_code_city" });

            migrationBuilder.CreateIndex(
                name: "IX_wilaya_zip_code",
                schema: "order_schema",
                table: "wilaya",
                column: "zip_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_address",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "address",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "city",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "wilaya",
                schema: "order_schema");
        }
    }
}
