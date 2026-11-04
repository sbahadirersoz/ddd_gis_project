using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Gis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Line_Table",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "LineString",
                table: "Line_Table",
                newName: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Line_Table",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "   ACTIVE =0,\n    INACTIVE =1,\n    SOFT_DELETED =2",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<LineString>(
                name: "Location",
                table: "Line_Table",
                type: "geography(LineString, 4326)",
                nullable: false,
                oldClrType: typeof(LineString),
                oldType: "geometry");

            migrationBuilder.CreateIndex(
                name: "IX_Line_Table_Location",
                table: "Line_Table",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Line_Table_Location",
                table: "Line_Table");

            migrationBuilder.DropIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Line_Table",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Line_Table",
                newName: "LineString");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Line_Table",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0,
                oldComment: "   ACTIVE =0,\n    INACTIVE =1,\n    SOFT_DELETED =2");

            migrationBuilder.AlterColumn<LineString>(
                name: "LineString",
                table: "Line_Table",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(LineString),
                oldType: "geography(LineString, 4326)");
        }
    }
}
