using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SynelTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Payroll_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Forenames = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date_of_Birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EMail_Home = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Payroll_Number);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
