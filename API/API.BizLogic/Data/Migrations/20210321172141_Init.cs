using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.BizLogic.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kyc");

            migrationBuilder.CreateTable(
                name: "UserDevices",
                schema: "kyc",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PushId = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDevices",
                schema: "kyc");
        }
    }
}
