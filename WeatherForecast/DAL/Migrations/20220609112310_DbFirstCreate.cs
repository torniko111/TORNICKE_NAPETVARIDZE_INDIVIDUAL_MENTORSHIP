using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DbFirstCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weathers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Lon = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Clouds = table.Column<string>(nullable: true),
                    TempC = table.Column<double>(nullable: false),
                    TempCMin = table.Column<double>(nullable: false),
                    TempCMax = table.Column<double>(nullable: false),
                    Pressure = table.Column<int>(nullable: false),
                    Humidity = table.Column<int>(nullable: false),
                    Visibility = table.Column<int>(nullable: false),
                    WindSpeed = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Sunrise = table.Column<long>(nullable: false),
                    Sunset = table.Column<long>(nullable: false),
                    CityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weathers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weathers");
        }
    }
}
