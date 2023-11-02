using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEBTP.Data.Migrations
{
    public partial class Reservations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Entregue",
                table: "Reservas",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "Aceite",
                table: "Reservas",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aceite",
                table: "Reservas");

            migrationBuilder.AlterColumn<bool>(
                name: "Entregue",
                table: "Reservas",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
