﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Order.Server.Persistence;

namespace Order.Server.Persistence.Migrations.V01
{
    [DbContext(typeof(OrderContext))]
    partial class OrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("order_schema")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:Collation", "English_United States.1252")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Order.DomainModel.Address", b =>
                {
                    b.Property<string>("Address1")
                        .HasColumnType("character varying")
                        .HasColumnName("address1");

                    b.Property<string>("Address2")
                        .HasColumnType("character varying")
                        .HasColumnName("address2");

                    b.Property<int>("IdCity")
                        .HasColumnType("integer")
                        .HasColumnName("id_city");

                    b.HasKey("Address1", "Address2", "IdCity")
                        .HasName("PK_ADDRESS");

                    b.HasIndex("IdCity");

                    b.ToTable("address", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("label");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("picture");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .HasDatabaseName("category_id_index");

                    b.ToTable("category", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("CodeWilaya")
                        .HasColumnType("integer")
                        .HasColumnName("code_wilaya");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<int>("ZipCode")
                        .HasColumnType("integer")
                        .HasColumnName("zip_code");

                    b.HasKey("Id")
                        .HasName("PK_CITY");

                    b.HasIndex("CodeWilaya");

                    b.ToTable("city", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("role", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("role_claim", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("LastName");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("user", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserAddress", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.Property<string>("Address1")
                        .HasColumnType("character varying")
                        .HasColumnName("address1");

                    b.Property<string>("Address2")
                        .HasColumnType("character varying")
                        .HasColumnName("address2");

                    b.Property<int>("IdCity")
                        .HasColumnType("integer")
                        .HasColumnName("id_city");

                    b.HasKey("IdUser", "Address1", "Address2", "IdCity")
                        .HasName("PK_USER_ADDRESS");

                    b.HasIndex("Address1", "Address2", "IdCity");

                    b.ToTable("user_address", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_claim", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("user_login", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserRefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("expire_at");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("UserId");

                    b.ToTable("user_refresh_token", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_role", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("user_token", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Wilaya", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("code")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<int>("ZipCode")
                        .HasColumnType("integer")
                        .HasColumnName("zip_code");

                    b.HasKey("Code")
                        .HasName("PK_WILAYA");

                    b.ToTable("wilaya", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Address", b =>
                {
                    b.HasOne("Order.DomainModel.City", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("IdCity")
                        .HasConstraintName("FK_ADDRESS_CITY")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Order.DomainModel.City", b =>
                {
                    b.HasOne("Order.DomainModel.Wilaya", "Wilaya")
                        .WithMany("Cities")
                        .HasForeignKey("CodeWilaya")
                        .HasConstraintName("FK_CITY_WILAYA")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wilaya");
                });

            modelBuilder.Entity("Order.DomainModel.RoleClaim", b =>
                {
                    b.HasOne("Order.DomainModel.Role", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Order.DomainModel.UserAddress", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("FK_USER_ADDRESS_USER")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Address", "Address")
                        .WithMany("UsersAddress")
                        .HasForeignKey("Address1", "Address2", "IdCity")
                        .HasConstraintName("FK_USER_ADDRESS_ADDRESS")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.UserClaim", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.UserLogin", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.UserRefreshToken", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("Order.DomainModel.UserRefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.UserRole", b =>
                {
                    b.HasOne("Order.DomainModel.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.UserToken", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.Address", b =>
                {
                    b.Navigation("UsersAddress");
                });

            modelBuilder.Entity("Order.DomainModel.City", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("Order.DomainModel.Role", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Order.DomainModel.User", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("RefreshToken");

                    b.Navigation("Tokens");

                    b.Navigation("UserAddresses");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Order.DomainModel.Wilaya", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
