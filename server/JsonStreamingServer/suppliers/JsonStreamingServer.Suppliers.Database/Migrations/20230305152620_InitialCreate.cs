using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JsonStreamingServer.Suppliers.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hotel_offer",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    short_description = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotel_offer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    url = table.Column<string>(type: "TEXT", nullable: false),
                    caption = table.Column<string>(type: "TEXT", nullable: true),
                    offer_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_image_hotel_offer_offer_id",
                        column: x => x.offer_id,
                        principalTable: "hotel_offer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pricing",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    day_from = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    day_to = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    age_from = table.Column<uint>(type: "INTEGER", nullable: false),
                    age_to = table.Column<uint>(type: "INTEGER", nullable: false),
                    value = table.Column<decimal>(type: "TEXT", nullable: false),
                    currency = table.Column<string>(type: "TEXT", nullable: false),
                    offer_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing", x => x.id);
                    table.ForeignKey(
                        name: "FK_pricing_hotel_offer_offer_id",
                        column: x => x.offer_id,
                        principalTable: "hotel_offer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_image_offer_id",
                table: "image",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_pricing_offer_id",
                table: "pricing",
                column: "offer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "pricing");

            migrationBuilder.DropTable(
                name: "hotel_offer");
        }
    }
}
