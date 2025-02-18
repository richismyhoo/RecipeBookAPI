using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBookAPI.Migrations
{
    /// <inheritdoc />
    public partial class countrytorecipemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Recipes");
        }
    }
}
