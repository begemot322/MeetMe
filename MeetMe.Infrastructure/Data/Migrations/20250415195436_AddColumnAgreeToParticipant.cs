using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetMe.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAgreeToParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAgreedWithFixedDate",
                table: "Participants",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgreedWithFixedDate",
                table: "Participants");
        }
    }
}
