using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoMottuMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class addRoleProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ROLE_FUNCIONARIO",
                table: "TB_GEOMOTTU_USUARIO",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ROLE_FUNCIONARIO",
                table: "TB_GEOMOTTU_USUARIO");
        }
    }
}
