using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyTravelManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unique_Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VehicleModelName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Create_By = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update_By = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Update_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Is_Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Travels",
                columns: table => new
                {
                    TravelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unique_Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    Kilometer = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    FuelCharge = table.Column<double>(type: "float", nullable: false),
                    MaintenanceCharge = table.Column<double>(type: "float", nullable: false),
                    DriverCommission = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pickup = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Drop = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Create_By = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update_By = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Update_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Is_Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Travels", x => x.TravelId);
                    table.ForeignKey(
                        name: "FK_tbl_Travels_tbl_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "tbl_Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Travels_VehicleId",
                table: "tbl_Travels",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Travels");

            migrationBuilder.DropTable(
                name: "tbl_Vehicles");
        }
    }
}
