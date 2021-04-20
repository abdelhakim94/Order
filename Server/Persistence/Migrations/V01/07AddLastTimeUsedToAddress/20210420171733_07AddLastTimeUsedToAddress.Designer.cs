﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Order.Server.Persistence;

namespace Order.Server.Persistence.Migrations.V01._07AddLastTimeUsedToAddress
{
    [DbContext(typeof(OrderContext))]
    [Migration("20210420171733_07AddLastTimeUsedToAddress")]
    partial class _07AddLastTimeUsedToAddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("ZipCodeCity")
                        .HasMaxLength(5)
                        .HasColumnType("character varying")
                        .HasColumnName("zip_code_city");

                    b.HasKey("Address1", "Address2", "ZipCodeCity")
                        .HasName("PK_ADDRESS");

                    b.HasIndex("ZipCodeCity");

                    b.ToTable("address", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK_CARD");

                    b.HasIndex("IdUser");

                    b.ToTable("card", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.CardDish", b =>
                {
                    b.Property<int>("IdDish")
                        .HasColumnType("integer")
                        .HasColumnName("id_dish");

                    b.Property<int>("IdCard")
                        .HasColumnType("integer")
                        .HasColumnName("id_card");

                    b.HasKey("IdDish", "IdCard")
                        .HasName("PK_CARD_DISH");

                    b.HasIndex("IdCard");

                    b.ToTable("card_dish", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.CardMenu", b =>
                {
                    b.Property<int>("IdCard")
                        .HasColumnType("integer")
                        .HasColumnName("id_card");

                    b.Property<int>("IdMenu")
                        .HasColumnType("integer")
                        .HasColumnName("id_menu");

                    b.HasKey("IdCard", "IdMenu")
                        .HasName("PK_CARD_MENU");

                    b.HasIndex("IdMenu");

                    b.ToTable("card_menu", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.CardSection", b =>
                {
                    b.Property<int>("IdSection")
                        .HasColumnType("integer")
                        .HasColumnName("id_section");

                    b.Property<int>("IdCard")
                        .HasColumnType("integer")
                        .HasColumnName("id_card");

                    b.HasKey("IdSection", "IdCard")
                        .HasName("PK_CARD_SECTION");

                    b.HasIndex("IdCard");

                    b.ToTable("card_section", "order_schema");
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
                    b.Property<string>("ZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying")
                        .HasColumnName("zip_code");

                    b.Property<string>("CodeWilaya")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying")
                        .HasColumnName("code_wilaya");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.HasKey("ZipCode")
                        .HasName("PK_CITY");

                    b.HasIndex("CodeWilaya");

                    b.HasIndex("Name")
                        .HasDatabaseName("INDEX_NAME_CITY");

                    b.ToTable("city", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasColumnType("character varying")
                        .HasColumnName("picture");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8, 2)")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("PK_DISH");

                    b.ToTable("dish", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.DishCategory", b =>
                {
                    b.Property<int>("IdCategory")
                        .HasColumnType("integer")
                        .HasColumnName("id_category");

                    b.Property<int>("IdDish")
                        .HasColumnType("integer")
                        .HasColumnName("id_dish");

                    b.HasKey("IdCategory", "IdDish")
                        .HasName("PK_DISH_CATEGORY");

                    b.HasIndex("IdDish");

                    b.ToTable("dish_category", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.DishSection", b =>
                {
                    b.Property<int>("IdDish")
                        .HasColumnType("integer")
                        .HasColumnName("id_dish");

                    b.Property<int>("IdSection")
                        .HasColumnType("integer")
                        .HasColumnName("id_section");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("boolean")
                        .HasColumnName("is_mandatory");

                    b.HasKey("IdDish", "IdSection")
                        .HasName("PK_DISH_SECTION");

                    b.HasIndex("IdSection");

                    b.ToTable("dish_section", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasColumnType("character varying")
                        .HasColumnName("picture");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8, 2)")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("PK_MENU");

                    b.ToTable("menu", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.MenuDish", b =>
                {
                    b.Property<int>("IdDish")
                        .HasColumnType("integer")
                        .HasColumnName("id_dish");

                    b.Property<int>("IdMenu")
                        .HasColumnType("integer")
                        .HasColumnName("id_menu");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("boolean");

                    b.HasKey("IdDish", "IdMenu")
                        .HasName("PK_MENU_DISH");

                    b.HasIndex("IdMenu");

                    b.ToTable("menu_dish", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.MenuSection", b =>
                {
                    b.Property<int>("IdMenu")
                        .HasColumnType("integer")
                        .HasColumnName("id_menu");

                    b.Property<int>("IdSection")
                        .HasColumnType("integer")
                        .HasColumnName("id_section");

                    b.HasKey("IdMenu", "IdSection")
                        .HasName("PK_MENU_SECTION");

                    b.HasIndex("IdSection");

                    b.ToTable("menu_section", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Profile", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK_PROFILE");

                    b.ToTable("profile", "order_schema");
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

            modelBuilder.Entity("Order.DomainModel.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK_SECTION");

                    b.ToTable("section", "order_schema");
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

                    b.Property<string>("ZipCodeCity")
                        .HasMaxLength(5)
                        .HasColumnType("character varying")
                        .HasColumnName("zip_code_city");

                    b.Property<DateTime>("LastTimeUsed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_time_used");

                    b.HasKey("IdUser", "Address1", "Address2", "ZipCodeCity")
                        .HasName("PK_USER_ADDRESS");

                    b.HasIndex("Address1", "Address2", "ZipCodeCity");

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

            modelBuilder.Entity("Order.DomainModel.UserProfile", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.Property<int>("IdProfile")
                        .HasColumnType("integer")
                        .HasColumnName("id_profile");

                    b.HasKey("IdUser", "IdProfile")
                        .HasName("PK_USER_PROFILE");

                    b.HasIndex("IdProfile");

                    b.ToTable("user_profile", "order_schema");
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
                    b.Property<string>("Code")
                        .HasMaxLength(2)
                        .HasColumnType("character varying")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying")
                        .HasColumnName("zip_code");

                    b.HasKey("Code")
                        .HasName("PK_WILAYA");

                    b.HasIndex("ZipCode")
                        .IsUnique();

                    b.ToTable("wilaya", "order_schema");
                });

            modelBuilder.Entity("Order.DomainModel.Address", b =>
                {
                    b.HasOne("Order.DomainModel.City", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("ZipCodeCity")
                        .HasConstraintName("FK_ADDRESS_CITY")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Order.DomainModel.Card", b =>
                {
                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("Cards")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("FK_CARD_USER")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Order.DomainModel.CardDish", b =>
                {
                    b.HasOne("Order.DomainModel.Card", "Card")
                        .WithMany("CardDishes")
                        .HasForeignKey("IdCard")
                        .HasConstraintName("FK_CARD_DISH_CARD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Dish", "Dish")
                        .WithMany("CardsDish")
                        .HasForeignKey("IdDish")
                        .HasConstraintName("FK_CARD_DISH_DISH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Order.DomainModel.CardMenu", b =>
                {
                    b.HasOne("Order.DomainModel.Card", "Card")
                        .WithMany("CardMenus")
                        .HasForeignKey("IdCard")
                        .HasConstraintName("FK_CARD_MENU_CARD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Menu", "Menu")
                        .WithMany("CardsMenu")
                        .HasForeignKey("IdMenu")
                        .HasConstraintName("FK_CARD_MENU_MENU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Order.DomainModel.CardSection", b =>
                {
                    b.HasOne("Order.DomainModel.Card", "Card")
                        .WithMany("CardSections")
                        .HasForeignKey("IdCard")
                        .HasConstraintName("FK_CARD_SECTION_CARD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Section", "Section")
                        .WithMany("CardsSection")
                        .HasForeignKey("IdSection")
                        .HasConstraintName("FK_CARD_SECTION_SECTION")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Section");
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

            modelBuilder.Entity("Order.DomainModel.DishCategory", b =>
                {
                    b.HasOne("Order.DomainModel.Category", "Category")
                        .WithMany("DishesCategory")
                        .HasForeignKey("IdCategory")
                        .HasConstraintName("FK_DISH_CATEGORY_CATEGORY")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Dish", "Dish")
                        .WithMany("DishCategories")
                        .HasForeignKey("IdDish")
                        .HasConstraintName("FK_DISH_CATEGORY_DISH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Order.DomainModel.DishSection", b =>
                {
                    b.HasOne("Order.DomainModel.Dish", "Dish")
                        .WithMany("DishSections")
                        .HasForeignKey("IdDish")
                        .HasConstraintName("FK_DISH_SECTION_DISH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Section", "Section")
                        .WithMany("DishesSection")
                        .HasForeignKey("IdSection")
                        .HasConstraintName("FK_DISH_SECTION_SECTION")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Order.DomainModel.MenuDish", b =>
                {
                    b.HasOne("Order.DomainModel.Dish", "Dish")
                        .WithMany("MenuesDish")
                        .HasForeignKey("IdDish")
                        .HasConstraintName("FK_MENU_DISH_DISH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Menu", "Menu")
                        .WithMany("MenuDishes")
                        .HasForeignKey("IdMenu")
                        .HasConstraintName("FK_MENU_DISH_MENU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Order.DomainModel.MenuSection", b =>
                {
                    b.HasOne("Order.DomainModel.Menu", "Menu")
                        .WithMany("MenuSections")
                        .HasForeignKey("IdMenu")
                        .HasConstraintName("FK_MENU_SECTION_MENU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.Section", "Section")
                        .WithMany("MenuesSection")
                        .HasForeignKey("IdSection")
                        .HasConstraintName("FK_MENU_SECTION_SECTION")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("Section");
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
                        .HasForeignKey("Address1", "Address2", "ZipCodeCity")
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

            modelBuilder.Entity("Order.DomainModel.UserProfile", b =>
                {
                    b.HasOne("Order.DomainModel.Profile", "Profile")
                        .WithMany("UsersProfile")
                        .HasForeignKey("IdProfile")
                        .HasConstraintName("FK_USER_PROFILE_PROFILE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order.DomainModel.User", "User")
                        .WithMany("UserProfiles")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("FK_USER_PROFILE_USER")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

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

            modelBuilder.Entity("Order.DomainModel.Card", b =>
                {
                    b.Navigation("CardDishes");

                    b.Navigation("CardMenus");

                    b.Navigation("CardSections");
                });

            modelBuilder.Entity("Order.DomainModel.Category", b =>
                {
                    b.Navigation("DishesCategory");
                });

            modelBuilder.Entity("Order.DomainModel.City", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("Order.DomainModel.Dish", b =>
                {
                    b.Navigation("CardsDish");

                    b.Navigation("DishCategories");

                    b.Navigation("DishSections");

                    b.Navigation("MenuesDish");
                });

            modelBuilder.Entity("Order.DomainModel.Menu", b =>
                {
                    b.Navigation("CardsMenu");

                    b.Navigation("MenuDishes");

                    b.Navigation("MenuSections");
                });

            modelBuilder.Entity("Order.DomainModel.Profile", b =>
                {
                    b.Navigation("UsersProfile");
                });

            modelBuilder.Entity("Order.DomainModel.Role", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Order.DomainModel.Section", b =>
                {
                    b.Navigation("CardsSection");

                    b.Navigation("DishesSection");

                    b.Navigation("MenuesSection");
                });

            modelBuilder.Entity("Order.DomainModel.User", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("RefreshToken");

                    b.Navigation("Tokens");

                    b.Navigation("UserAddresses");

                    b.Navigation("UserProfiles");

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
