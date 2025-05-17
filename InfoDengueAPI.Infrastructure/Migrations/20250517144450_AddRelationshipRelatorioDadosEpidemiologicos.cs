using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDengueAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipRelatorioDadosEpidemiologicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Solicitantes_CPF",
                table: "Solicitantes");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Solicitantes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Solicitantes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_CPF",
                table: "Solicitantes",
                column: "CPF",
                unique: true);
        }
    }
}
