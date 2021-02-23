using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Order.IdentityServer.Persistence.Migrations.V01.CreateISTables
{
    public partial class CreateISTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order_schema");

            migrationBuilder.CreateTable(
                name: "device_flow_code",
                schema: "order_schema",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_flow_code", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_resource",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_scope",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_scope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "is4_client",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProtocolType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(type: "boolean", nullable: false),
                    ClientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ClientUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RequireConsent = table.Column<bool>(type: "boolean", nullable: false),
                    AllowRememberConsent = table.Column<bool>(type: "boolean", nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "boolean", nullable: false),
                    RequirePkce = table.Column<bool>(type: "boolean", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(type: "boolean", nullable: false),
                    RequireRequestObject = table.Column<bool>(type: "boolean", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "boolean", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                    BackChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                    AllowOfflineAccess = table.Column<bool>(type: "boolean", nullable: false),
                    IdentityTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccessTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                    ConsentLifetime = table.Column<int>(type: "integer", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "integer", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "integer", nullable: false),
                    AccessTokenType = table.Column<int>(type: "integer", nullable: false),
                    EnableLocalLogin = table.Column<bool>(type: "boolean", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "boolean", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "boolean", nullable: false),
                    ClientClaimsPrefix = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserSsoLifetime = table.Column<int>(type: "integer", nullable: true),
                    UserCodeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeviceCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "is4_identity_resource",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_identity_resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persisted_grant",
                schema: "order_schema",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persisted_grant", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_resource_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_resource_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_resource_claim_is4_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_resource_properties",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_resource_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_resource_properties_is4_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_resource_scope",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_resource_scope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_resource_scope_is4_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_resource_secret",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_resource_secret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_resource_secret_is4_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_scope_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_scope_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_scope_claim_is4_api_scope_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_api_scope_property",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_api_scope_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_api_scope_property_is4_api_scope_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "order_schema",
                        principalTable: "is4_api_scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_claim_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_cors_origin",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Origin = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_cors_origin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_cors_origin_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_grant_type",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_grant_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_grant_type_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_idp_restriction",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Provider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_idp_restriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_idp_restriction_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_post_logout_redirection_uri",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostLogoutRedirectUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_post_logout_redirection_uri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_post_logout_redirection_uri_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_property",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_property_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_redirect_uri",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RedirectUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_redirect_uri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_redirect_uri_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_scope",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_scope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_scope_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_client_secret",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_client_secret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_client_secret_is4_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "order_schema",
                        principalTable: "is4_client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_identity_resource_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_identity_resource_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_identity_resource_claim_is4_identity_resource_IdentityR~",
                        column: x => x.IdentityResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_identity_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "is4_identity_resource_property",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_is4_identity_resource_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_is4_identity_resource_property_is4_identity_resource_Identi~",
                        column: x => x.IdentityResourceId,
                        principalSchema: "order_schema",
                        principalTable: "is4_identity_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_claim_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "order_schema",
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claim",
                schema: "order_schema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_claim_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                schema: "order_schema",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_user_login_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "order_schema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_role_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "order_schema",
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                schema: "order_schema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_user_token_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "order_schema",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_flow_code_DeviceCode",
                schema: "order_schema",
                table: "device_flow_code",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_flow_code_Expiration",
                schema: "order_schema",
                table: "device_flow_code",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_resource_Name",
                schema: "order_schema",
                table: "is4_api_resource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_resource_claim_ApiResourceId",
                schema: "order_schema",
                table: "is4_api_resource_claim",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_resource_properties_ApiResourceId",
                schema: "order_schema",
                table: "is4_api_resource_properties",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_resource_scope_ApiResourceId",
                schema: "order_schema",
                table: "is4_api_resource_scope",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_resource_secret_ApiResourceId",
                schema: "order_schema",
                table: "is4_api_resource_secret",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_scope_Name",
                schema: "order_schema",
                table: "is4_api_scope",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_scope_claim_ScopeId",
                schema: "order_schema",
                table: "is4_api_scope_claim",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_api_scope_property_ScopeId",
                schema: "order_schema",
                table: "is4_api_scope_property",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_ClientId",
                schema: "order_schema",
                table: "is4_client",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_claim_ClientId",
                schema: "order_schema",
                table: "is4_client_claim",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_cors_origin_ClientId",
                schema: "order_schema",
                table: "is4_client_cors_origin",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_grant_type_ClientId",
                schema: "order_schema",
                table: "is4_client_grant_type",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_idp_restriction_ClientId",
                schema: "order_schema",
                table: "is4_client_idp_restriction",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_post_logout_redirection_uri_ClientId",
                schema: "order_schema",
                table: "is4_client_post_logout_redirection_uri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_property_ClientId",
                schema: "order_schema",
                table: "is4_client_property",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_redirect_uri_ClientId",
                schema: "order_schema",
                table: "is4_client_redirect_uri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_scope_ClientId",
                schema: "order_schema",
                table: "is4_client_scope",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_client_secret_ClientId",
                schema: "order_schema",
                table: "is4_client_secret",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_identity_resource_Name",
                schema: "order_schema",
                table: "is4_identity_resource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_is4_identity_resource_claim_IdentityResourceId",
                schema: "order_schema",
                table: "is4_identity_resource_claim",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_is4_identity_resource_property_IdentityResourceId",
                schema: "order_schema",
                table: "is4_identity_resource_property",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_persisted_grant_Expiration",
                schema: "order_schema",
                table: "persisted_grant",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_persisted_grant_SubjectId_ClientId_Type",
                schema: "order_schema",
                table: "persisted_grant",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_persisted_grant_SubjectId_SessionId_Type",
                schema: "order_schema",
                table: "persisted_grant",
                columns: new[] { "SubjectId", "SessionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "order_schema",
                table: "role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_claim_RoleId",
                schema: "order_schema",
                table: "role_claim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "order_schema",
                table: "user",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "order_schema",
                table: "user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_claim_UserId",
                schema: "order_schema",
                table: "user_claim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_UserId",
                schema: "order_schema",
                table: "user_login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_RoleId",
                schema: "order_schema",
                table: "user_role",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_flow_code",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_resource_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_resource_properties",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_resource_scope",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_resource_secret",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_scope_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_scope_property",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_cors_origin",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_grant_type",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_idp_restriction",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_post_logout_redirection_uri",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_property",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_redirect_uri",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_scope",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client_secret",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_identity_resource_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_identity_resource_property",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "persisted_grant",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "role_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "user_claim",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "user_login",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "user_token",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_resource",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_api_scope",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_client",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "is4_identity_resource",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "role",
                schema: "order_schema");

            migrationBuilder.DropTable(
                name: "user",
                schema: "order_schema");
        }
    }
}
