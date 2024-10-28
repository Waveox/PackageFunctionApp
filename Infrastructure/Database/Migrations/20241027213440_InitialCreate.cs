using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFunctionApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    SupplierId = table.Column<string>(type: "TEXT", nullable: false),
                    PackageId = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => new { x.SupplierId, x.PackageId });
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PoNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PackageId = table.Column<string>(type: "TEXT", nullable: true),
                    PackageSupplierId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Packages_PackageSupplierId_PackageId",
                        columns: x => new { x.PackageSupplierId, x.PackageId },
                        principalTable: "Packages",
                        principalColumns: new[] { "SupplierId", "PackageId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Barcode",
                table: "Items",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Items_PackageSupplierId_PackageId",
                table: "Items",
                columns: new[] { "PackageSupplierId", "PackageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_PoNumber",
                table: "Items",
                column: "PoNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_SupplierId_PackageId",
                table: "Packages",
                columns: new[] { "SupplierId", "PackageId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
