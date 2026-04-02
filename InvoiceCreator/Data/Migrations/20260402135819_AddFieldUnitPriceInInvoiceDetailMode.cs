using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceCreator.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldUnitPriceInInvoiceDetailMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceDetails",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InvoiceDetails");
        }
    }
}
