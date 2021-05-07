using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Order.Server.Persistence.Migrations.V01._09DeleteZipCodeForCity
{
    public partial class _09DeleteZipCodeForCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ADDRESS_CITY",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_CITY_WILAYA",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropForeignKey(
                name: "FK_USER_ADDRESS_ADDRESS",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WILAYA",
                schema: "order_schema",
                table: "wilaya");

            migrationBuilder.DropIndex(
                name: "IX_wilaya_zip_code",
                schema: "order_schema",
                table: "wilaya");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER_ADDRESS",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropIndex(
                name: "IX_user_address_address1_address2_zip_code_city",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CITY",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropIndex(
                name: "IX_city_code_wilaya",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ADDRESS",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropIndex(
                name: "IX_address_zip_code_city",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropColumn(
                name: "code",
                schema: "order_schema",
                table: "wilaya");

            migrationBuilder.DropColumn(
                name: "zip_code_city",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropColumn(
                name: "zip_code",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropColumn(
                name: "code_wilaya",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropColumn(
                name: "zip_code_city",
                schema: "order_schema",
                table: "address");

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "order_schema",
                table: "wilaya",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id_city",
                schema: "order_schema",
                table: "user_address",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "order_schema",
                table: "city",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id_wilaya",
                schema: "order_schema",
                table: "city",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_city",
                schema: "order_schema",
                table: "address",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WILAYA",
                schema: "order_schema",
                table: "wilaya",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER_ADDRESS",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "id_user", "address1", "address2", "id_city" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CITY",
                schema: "order_schema",
                table: "city",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ADDRESS",
                schema: "order_schema",
                table: "address",
                columns: new[] { "address1", "address2", "id_city" });

            migrationBuilder.CreateIndex(
                name: "IX_user_address_address1_address2_id_city",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "address1", "address2", "id_city" });

            migrationBuilder.CreateIndex(
                name: "IX_city_id_wilaya",
                schema: "order_schema",
                table: "city",
                column: "id_wilaya");

            migrationBuilder.CreateIndex(
                name: "IX_address_id_city",
                schema: "order_schema",
                table: "address",
                column: "id_city");

            migrationBuilder.AddForeignKey(
                name: "FK_ADDRESS_CITY",
                schema: "order_schema",
                table: "address",
                column: "id_city",
                principalSchema: "order_schema",
                principalTable: "city",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CITY_WILAYA",
                schema: "order_schema",
                table: "city",
                column: "id_wilaya",
                principalSchema: "order_schema",
                principalTable: "wilaya",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USER_ADDRESS_ADDRESS",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "address1", "address2", "id_city" },
                principalSchema: "order_schema",
                principalTable: "address",
                principalColumns: new[] { "address1", "address2", "id_city" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ADDRESS_CITY",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_CITY_WILAYA",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropForeignKey(
                name: "FK_USER_ADDRESS_ADDRESS",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WILAYA",
                schema: "order_schema",
                table: "wilaya");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER_ADDRESS",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropIndex(
                name: "IX_user_address_address1_address2_id_city",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CITY",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropIndex(
                name: "IX_city_id_wilaya",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ADDRESS",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropIndex(
                name: "IX_address_id_city",
                schema: "order_schema",
                table: "address");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "order_schema",
                table: "wilaya");

            migrationBuilder.DropColumn(
                name: "id_city",
                schema: "order_schema",
                table: "user_address");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropColumn(
                name: "id_wilaya",
                schema: "order_schema",
                table: "city");

            migrationBuilder.DropColumn(
                name: "id_city",
                schema: "order_schema",
                table: "address");

            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "order_schema",
                table: "wilaya",
                type: "character varying",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "zip_code_city",
                schema: "order_schema",
                table: "user_address",
                type: "character varying",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "zip_code",
                schema: "order_schema",
                table: "city",
                type: "character varying",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "code_wilaya",
                schema: "order_schema",
                table: "city",
                type: "character varying",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "zip_code_city",
                schema: "order_schema",
                table: "address",
                type: "character varying",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WILAYA",
                schema: "order_schema",
                table: "wilaya",
                column: "code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER_ADDRESS",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "id_user", "address1", "address2", "zip_code_city" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CITY",
                schema: "order_schema",
                table: "city",
                column: "zip_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ADDRESS",
                schema: "order_schema",
                table: "address",
                columns: new[] { "address1", "address2", "zip_code_city" });

            migrationBuilder.CreateIndex(
                name: "IX_wilaya_zip_code",
                schema: "order_schema",
                table: "wilaya",
                column: "zip_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_address_address1_address2_zip_code_city",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "address1", "address2", "zip_code_city" });

            migrationBuilder.CreateIndex(
                name: "IX_city_code_wilaya",
                schema: "order_schema",
                table: "city",
                column: "code_wilaya");

            migrationBuilder.CreateIndex(
                name: "IX_address_zip_code_city",
                schema: "order_schema",
                table: "address",
                column: "zip_code_city");

            migrationBuilder.AddForeignKey(
                name: "FK_ADDRESS_CITY",
                schema: "order_schema",
                table: "address",
                column: "zip_code_city",
                principalSchema: "order_schema",
                principalTable: "city",
                principalColumn: "zip_code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CITY_WILAYA",
                schema: "order_schema",
                table: "city",
                column: "code_wilaya",
                principalSchema: "order_schema",
                principalTable: "wilaya",
                principalColumn: "code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_USER_ADDRESS_ADDRESS",
                schema: "order_schema",
                table: "user_address",
                columns: new[] { "address1", "address2", "zip_code_city" },
                principalSchema: "order_schema",
                principalTable: "address",
                principalColumns: new[] { "address1", "address2", "zip_code_city" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
