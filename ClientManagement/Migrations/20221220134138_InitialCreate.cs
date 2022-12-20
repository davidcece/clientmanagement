using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cellphone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    InternationalPhone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    EmailStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SmsStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.CheckConstraint("CheckEmailStatus", "[EmailStatus] IN('Active','Removed')");
                    table.CheckConstraint("CheckSmsStatus", "[SmsStatus] IN('Active','Removed')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email_InternationalPhone",
                table: "Clients",
                columns: new[] { "Email", "InternationalPhone" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
