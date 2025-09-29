using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_App.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPriceToPositive1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Book_Price",
                table: "Books",
                sql: "[Price] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Book_Price",
                table: "Books");
        }
    }
}
