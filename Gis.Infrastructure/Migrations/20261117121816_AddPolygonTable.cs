using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Gis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPolygonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polygon_Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Polygon>(type: "geography(Polygon, 4326)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false),
                    Wkt = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "   ACTIVE =0,\n    INACTIVE =1,\n    SOFT_DELETED =2")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polygon_Table", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polygon_Table_Location",
                table: "Polygon_Table",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_Polygon_Table_Name",
                table: "Polygon_Table",
                column: "Name",
                unique: true,
                filter: "\"status\" <> 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Polygon_Table");
        }
    }
}
