using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace gis.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Line_Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LineString = table.Column<LineString>(type: "geometry", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false),
                    Wkt = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line_Table", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poi_Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Point>(type: "geography(Point, 4326)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false),
                    Wkt = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "   ACTIVE =0,\n    INACTIVE =1,\n    SOFT_DELETED =2")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poi_Table", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poi_Table_Location",
                table: "Poi_Table",
                column: "Location",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Poi_Table_Name",
                table: "Poi_Table",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Line_Table");

            migrationBuilder.DropTable(
                name: "Poi_Table");
        }
    }
}
