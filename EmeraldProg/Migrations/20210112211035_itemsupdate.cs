using Microsoft.EntityFrameworkCore.Migrations;

namespace EmeraldProg.Migrations
{
    public partial class itemsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Items",
                newName: "ItemPrice");

            migrationBuilder.AddColumn<double>(
                name: "InstalPrice",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstalPrice",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemPrice",
                table: "Items",
                newName: "Price");
        }
    }
}
