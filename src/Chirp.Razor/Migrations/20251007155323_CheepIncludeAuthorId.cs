using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Razor.Migrations
{
    /// <inheritdoc />
    public partial class CheepIncludeAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Cheeps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Cheeps",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId");
        }
    }
}
