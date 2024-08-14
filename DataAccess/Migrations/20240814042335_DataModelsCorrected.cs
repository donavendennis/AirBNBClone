using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DataModelsCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Rental",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "Photos",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RentalId",
                table: "Reservation",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_OwnerId",
                table: "Rental",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_OwnerId",
                table: "Rental",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Rental_RentalId",
                table: "Reservation",
                column: "RentalId",
                principalTable: "Rental",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_OwnerId",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Rental_RentalId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_RentalId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Rental_OwnerId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Rental");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "Photos",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
