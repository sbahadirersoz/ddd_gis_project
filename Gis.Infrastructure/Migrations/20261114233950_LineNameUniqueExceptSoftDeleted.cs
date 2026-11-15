using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LineNameUniqueExceptSoftDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table");

            migrationBuilder.CreateIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table",
                column: "Name",
                unique: true,
                filter: "\"status\" <> 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table");

            migrationBuilder.CreateIndex(
                name: "IX_Line_Table_Name",
                table: "Line_Table",
                column: "Name",
                unique: true);
        }
    }
}
