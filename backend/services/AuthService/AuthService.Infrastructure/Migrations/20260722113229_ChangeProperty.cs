using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpVerifications",
                table: "OtpVerifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpVerifications",
                table: "OtpVerifications",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpVerifications",
                table: "OtpVerifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpVerifications",
                table: "OtpVerifications",
                column: "PhoneNumber");
        }
    }
}
