using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEBTP.Data.Migrations
{
    public partial class ReservaCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId1",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_FuncionarioId1",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_FuncionarioId",
                table: "Reservas",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId",
                table: "Reservas",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_FuncionarioId",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "Reservas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Funcionarios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_FuncionarioId1",
                table: "Reservas",
                column: "FuncionarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId1",
                table: "Reservas",
                column: "FuncionarioId1",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
