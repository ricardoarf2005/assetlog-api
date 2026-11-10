using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetLog.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descritpion",
                table: "Equipments",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Equipments",
                newName: "Descritpion");
        }
    }
}
