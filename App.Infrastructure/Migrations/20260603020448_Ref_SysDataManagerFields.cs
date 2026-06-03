using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AapRepository.Migrations
{
    /// <inheritdoc />
    public partial class Ref_SysDataManagerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ref_SysDataManagerFields",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataManagerTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ColumnHeading = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShowOrderNumber = table.Column<int>(type: "int", nullable: false),
                    DefaultWidth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsScreenType = table.Column<bool>(type: "bit", nullable: false),
                    UniqueNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    RefAddedByID = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefEditedByID = table.Column<int>(type: "int", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ref_SysDataManagerFields", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ref_SysDataManagerFields");
        }
    }
}
