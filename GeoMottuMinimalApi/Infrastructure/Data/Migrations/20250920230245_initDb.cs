using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoMottuMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_GEOMOTTU_FILIAL",
                columns: table => new
                {
                    ID_FILIAL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_FILIAL = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PAIS_FILIAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ESTADO_FILIAL = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ENDERECO_FILIAL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GEOMOTTU_FILIAL", x => x.ID_FILIAL);
                });

            migrationBuilder.CreateTable(
                name: "TB_GEOMOTTU_PATIO",
                columns: table => new
                {
                    ID_PATIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CAPC_PATIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REFERENCIA_PATIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    TAMANHO_PATIO = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TipoDoPatio = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FilialId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GEOMOTTU_PATIO", x => x.ID_PATIO);
                    table.ForeignKey(
                        name: "FK_TB_GEOMOTTU_PATIO_TB_GEOMOTTU_FILIAL_FilialId",
                        column: x => x.FilialId,
                        principalTable: "TB_GEOMOTTU_FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_GEOMOTTU_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL_FUNCIONARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    SENHA_FUNCIONARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FilialId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CadastradoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GEOMOTTU_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_GEOMOTTU_USUARIO_TB_GEOMOTTU_FILIAL_FilialId",
                        column: x => x.FilialId,
                        principalTable: "TB_GEOMOTTU_FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_GEOMOTTU_MOTO",
                columns: table => new
                {
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLACA_MOTO = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    CHASSI_MOTO = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    CD_IOT_PLACA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    MOTO_MODELO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MOTOR_MOTO = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    MOTO_PROPRIETARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    PosicaoX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PosicaoY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PatioId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GEOMOTTU_MOTO", x => x.ID_MOTO);
                    table.ForeignKey(
                        name: "FK_TB_GEOMOTTU_MOTO_TB_GEOMOTTU_PATIO_PatioId",
                        column: x => x.PatioId,
                        principalTable: "TB_GEOMOTTU_PATIO",
                        principalColumn: "ID_PATIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_MOTO_CD_IOT_PLACA",
                table: "TB_GEOMOTTU_MOTO",
                column: "CD_IOT_PLACA",
                unique: true,
                filter: "\"CD_IOT_PLACA\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_MOTO_CHASSI_MOTO",
                table: "TB_GEOMOTTU_MOTO",
                column: "CHASSI_MOTO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_MOTO_PatioId",
                table: "TB_GEOMOTTU_MOTO",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_MOTO_PLACA_MOTO",
                table: "TB_GEOMOTTU_MOTO",
                column: "PLACA_MOTO",
                unique: true,
                filter: "\"PLACA_MOTO\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_PATIO_FilialId",
                table: "TB_GEOMOTTU_PATIO",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_USUARIO_EMAIL_FUNCIONARIO",
                table: "TB_GEOMOTTU_USUARIO",
                column: "EMAIL_FUNCIONARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_GEOMOTTU_USUARIO_FilialId",
                table: "TB_GEOMOTTU_USUARIO",
                column: "FilialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_GEOMOTTU_MOTO");

            migrationBuilder.DropTable(
                name: "TB_GEOMOTTU_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_GEOMOTTU_PATIO");

            migrationBuilder.DropTable(
                name: "TB_GEOMOTTU_FILIAL");
        }
    }
}
