using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDengueAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDadoEpidemiologicoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DadosEpidemiologicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicioSE = table.Column<long>(type: "bigint", nullable: false),
                    SE = table.Column<int>(type: "int", nullable: false),
                    CasosEst = table.Column<double>(type: "float", nullable: false),
                    CasosEstMin = table.Column<int>(type: "int", nullable: false),
                    CasosEstMax = table.Column<int>(type: "int", nullable: false),
                    Casos = table.Column<int>(type: "int", nullable: false),
                    Prt1 = table.Column<double>(type: "float", nullable: false),
                    Pinc100k = table.Column<double>(type: "float", nullable: false),
                    LocalidadeId = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    VersaoModelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rt = table.Column<double>(type: "float", nullable: true),
                    Populacao = table.Column<double>(type: "float", nullable: false),
                    TempMin = table.Column<double>(type: "float", nullable: false),
                    UmidMax = table.Column<double>(type: "float", nullable: false),
                    Receptivo = table.Column<int>(type: "int", nullable: false),
                    Transmissao = table.Column<int>(type: "int", nullable: false),
                    NivelInc = table.Column<int>(type: "int", nullable: false),
                    UmidMed = table.Column<double>(type: "float", nullable: false),
                    UmidMin = table.Column<double>(type: "float", nullable: false),
                    TempMed = table.Column<double>(type: "float", nullable: false),
                    TempMax = table.Column<double>(type: "float", nullable: false),
                    CasosProv = table.Column<int>(type: "int", nullable: false),
                    CasosProvEst = table.Column<int>(type: "int", nullable: true),
                    CasosProvEstMin = table.Column<int>(type: "int", nullable: true),
                    CasosProvEstMax = table.Column<int>(type: "int", nullable: true),
                    CasosConf = table.Column<int>(type: "int", nullable: true),
                    NotifAccumYear = table.Column<int>(type: "int", nullable: false),
                    RelatorioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosEpidemiologicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosEpidemiologicos_Relatorios_RelatorioId",
                        column: x => x.RelatorioId,
                        principalTable: "Relatorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DadosEpidemiologicos_RelatorioId",
                table: "DadosEpidemiologicos",
                column: "RelatorioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosEpidemiologicos");
        }
    }
}
