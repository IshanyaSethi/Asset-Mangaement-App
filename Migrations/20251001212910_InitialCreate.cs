using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssetName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AssetType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MakeModel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WarrantyExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsSpare = table.Column<bool>(type: "INTEGER", nullable: false),
                    Specifications = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "AssetAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "AssetId", "AssetName", "AssetType", "Condition", "IsSpare", "MakeModel", "PurchaseDate", "SerialNumber", "Specifications", "Status", "WarrantyExpiryDate" },
                values: new object[,]
                {
                    { 1, "Dell Laptop XPS 15", "Laptop", "Good", false, "Dell XPS 15 9520", new DateTime(2024, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5030), "DL-XPS-001", "Intel i7, 16GB RAM, 512GB SSD", "Available", new DateTime(2027, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5040) },
                    { 2, "HP Laptop ProBook", "Laptop", "Good", false, "HP ProBook 450 G9", new DateTime(2023, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5050), "HP-PRO-002", "Intel i5, 8GB RAM, 256GB SSD", "Available", new DateTime(2026, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5050) },
                    { 3, "Samsung Monitor 27\"", "Monitor", "Good", false, "Samsung 27\" LED", new DateTime(2024, 4, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5060), "SM-MON-003", "27 inch, 1920x1080, IPS Panel", "Available", new DateTime(2026, 4, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5060) },
                    { 4, "LG Monitor 24\"", "Monitor", "Good", true, "LG 24\" LED", new DateTime(2024, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5060), "LG-MON-004", "24 inch, 1920x1080", "Available", new DateTime(2027, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5070) },
                    { 5, "Logitech Keyboard", "Keyboard", "New", false, "Logitech K380", new DateTime(2025, 4, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5070), "LG-KB-005", "Wireless, Bluetooth", "Available", new DateTime(2026, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5070) },
                    { 6, "Logitech Mouse", "Mouse", "Good", true, "Logitech MX Master 3", new DateTime(2025, 2, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5080), "LG-MS-006", "Wireless, Ergonomic", "Available", new DateTime(2027, 2, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5080) },
                    { 7, "iPhone 13", "Mobile Phone", "Good", false, "Apple iPhone 13", new DateTime(2024, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5080), "APL-IP-007", "128GB, Blue", "Available", new DateTime(2025, 11, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5080) },
                    { 8, "MacBook Pro 14", "Laptop", "New", false, "Apple MacBook Pro 14", new DateTime(2025, 7, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5090), "APL-MBP-008", "M2 Pro, 16GB RAM, 512GB SSD", "Under Repair", new DateTime(2028, 7, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5090) },
                    { 9, "Dell Monitor 32\"", "Monitor", "Needs Repair", false, "Dell UltraSharp 32", new DateTime(2022, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5090), "DL-MON-009", "32 inch, 4K", "Retired", new DateTime(2025, 4, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5090) },
                    { 10, "HP Laptop EliteBook", "Laptop", "Good", false, "HP EliteBook 840 G8", new DateTime(2024, 12, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5100), "HP-ELT-010", "Intel i7, 16GB RAM, 512GB SSD", "Available", new DateTime(2027, 10, 2, 2, 59, 10, 769, DateTimeKind.Local).AddTicks(5100) }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Department", "Designation", "Email", "FullName", "IsActive", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "IT", "Senior Developer", "john.doe@company.com", "John Doe", true, "1234567890" },
                    { 2, "HR", "HR Manager", "jane.smith@company.com", "Jane Smith", true, "0987654321" },
                    { 3, "IT", "Junior Developer", "mike.johnson@company.com", "Mike Johnson", true, "5551234567" },
                    { 4, "Finance", "Accountant", "sarah.williams@company.com", "Sarah Williams", true, "5559876543" },
                    { 5, "IT", "DevOps Engineer", "david.brown@company.com", "David Brown", false, "5556781234" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_AssetId",
                table: "AssetAssignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_EmployeeId",
                table: "AssetAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_SerialNumber",
                table: "Assets",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAssignments");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
