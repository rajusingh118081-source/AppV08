using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AapRepository.Migrations
{
    /// <inheritdoc />
    public partial class CustomerAndInvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Main_Contacts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryContactID = table.Column<int>(type: "int", nullable: true),
                    ContactPhoto = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RefContactTypeID = table.Column<int>(type: "int", nullable: false),
                    RefRegionID = table.Column<int>(type: "int", nullable: true),
                    RefAssignedToID = table.Column<int>(type: "int", nullable: false),
                    RefCampaignID = table.Column<int>(type: "int", nullable: true),
                    IsBillWithParent = table.Column<bool>(type: "bit", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    BillAddressLine1 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BillAddressLine2 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BillAddressLine3 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BillCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BillStateProvince = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BillZipPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BillCountry = table.Column<int>(type: "int", nullable: false),
                    ShipAddressLine1 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ShipAddressLine2 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ShipAdressLine3 = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ShipCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShipStateProvince = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShipZipPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ShipCountry = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalBilled = table.Column<decimal>(type: "DECIMAL(20,4)", nullable: false),
                    TotalPaid = table.Column<decimal>(type: "DECIMAL(20,4)", nullable: false),
                    TotalBalanceDue = table.Column<decimal>(type: "DECIMAL(20,4)", nullable: false),
                    SalesRep = table.Column<int>(type: "int", nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    RefAddedByID = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefEditedByID = table.Column<int>(type: "int", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Main_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Main_Invoices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QboInvoiceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerQboID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    RefAddedByID = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefEditedByID = table.Column<int>(type: "int", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Main_Invoices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Main_InvoiceLineItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkedInvoiceID = table.Column<int>(type: "int", nullable: false),
                    QboLineID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QboItemID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    RefAddedByID = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefEditedByID = table.Column<int>(type: "int", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Main_InvoiceLineItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Main_InvoiceLineItems_Main_Invoices_LinkedInvoiceID",
                        column: x => x.LinkedInvoiceID,
                        principalTable: "Main_Invoices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Main_InvoiceLineItems_LinkedInvoiceID",
                table: "Main_InvoiceLineItems",
                column: "LinkedInvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Main_Invoices_QboInvoiceID",
                table: "Main_Invoices",
                column: "QboInvoiceID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Main_Contacts");

            migrationBuilder.DropTable(
                name: "Main_InvoiceLineItems");

            migrationBuilder.DropTable(
                name: "Main_Invoices");
        }
    }
}
