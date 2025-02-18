using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBookAPI.Migrations
{
    /// <inheritdoc />
    public partial class spicyandcomplexity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Complexity",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpicyLevel",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeToCompleteInMinutes",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complexity",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SpicyLevel",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TimeToCompleteInMinutes",
                table: "Recipes");
        }
    }
}
