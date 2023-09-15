using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIsPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                schema: "Orders",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                schema: "Orders",
                table: "Orders");
        }
    }
}
