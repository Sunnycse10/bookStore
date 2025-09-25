using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_App.Migrations
{
    /// <inheritdoc />
    public partial class ChangedISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN_10",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ISBN_10",
                table: "Books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
