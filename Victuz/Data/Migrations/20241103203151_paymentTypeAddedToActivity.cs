using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Data.Migrations
{
    /// <inheritdoc />
    public partial class paymentTypeAddedToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Activities");
        }
    }
}
