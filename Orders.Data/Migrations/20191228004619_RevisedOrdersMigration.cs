using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.Data.Migrations
{
    public partial class RevisedOrdersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderProfiles_OrderProfileId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderProfileId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDispatches_OrderId",
                table: "OrderDispatches");

            migrationBuilder.DropColumn(
                name: "OrderProfileId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProfileId",
                table: "Orders",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDispatches_OrderId",
                table: "OrderDispatches",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Profiles_ProfileId",
                table: "Orders",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Profiles_ProfileId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProfileId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDispatches_OrderId",
                table: "OrderDispatches");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderProfileId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderProfileId",
                table: "Orders",
                column: "OrderProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDispatches_OrderId",
                table: "OrderDispatches",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderProfiles_OrderProfileId",
                table: "Orders",
                column: "OrderProfileId",
                principalTable: "OrderProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
