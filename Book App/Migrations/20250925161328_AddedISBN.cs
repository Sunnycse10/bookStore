using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_App.Migrations
{
    /// <inheritdoc />
    public partial class AddedISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN_10",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN_10",
                table: "Books");
        }
    }
}
