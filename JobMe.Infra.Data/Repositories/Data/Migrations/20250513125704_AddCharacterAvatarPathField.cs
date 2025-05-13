using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMe.Infra.Data.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterAvatarPathField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "Characters",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "Characters");
        }
    }
}
