using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gis.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class f1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Poi_Table_Location",
                table: "Poi_Table");

            migrationBuilder.CreateIndex(
                name: "IX_Poi_Table_Location",
                table: "Poi_Table",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "GIST");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Poi_Table_Location",
                table: "Poi_Table");

            migrationBuilder.CreateIndex(
                name: "IX_Poi_Table_Location",
                table: "Poi_Table",
                column: "Location",
                unique: true);
        }
    }
}
