using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Puchase_and_payables.Migrations
{
    public partial class SupplierObj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cor_suppliertype",
                columns: table => new
                {
                    SupplierTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierTypeName = table.Column<string>(maxLength: 250, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_suppliertype", x => x.SupplierTypeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_topclient",
                columns: table => new
                {
                    TopClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 50, nullable: true),
                    NoOfStaff = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_topclient", x => x.TopClientId);
                });

            migrationBuilder.CreateTable(
                name: "cor_supplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Passport = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    RegistrationNo = table.Column<string>(maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    TaxIDorVATID = table.Column<string>(nullable: true),
                    PostalAddress = table.Column<string>(nullable: true),
                    SupplierNumber = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    HaveWorkPrintPermit = table.Column<bool>(nullable: false),
                    cor_suppliertypeSupplierTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_supplier", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_cor_supplier_cor_suppliertype_cor_suppliertypeSupplierTypeId",
                        column: x => x.cor_suppliertypeSupplierTypeId,
                        principalTable: "cor_suppliertype",
                        principalColumn: "SupplierTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_supplierauthorization",
                columns: table => new
                {
                    SupplierAuthorizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    cor_supplierSupplierId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_supplierauthorization", x => x.SupplierAuthorizationId);
                    table.ForeignKey(
                        name: "FK_cor_supplierauthorization_cor_supplier_cor_supplierSupplierId",
                        column: x => x.cor_supplierSupplierId,
                        principalTable: "cor_supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_supplierbusinessowner",
                columns: table => new
                {
                    SupplierBusinessOwnerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    cor_supplierSupplierId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_supplierbusinessowner", x => x.SupplierBusinessOwnerId);
                    table.ForeignKey(
                        name: "FK_cor_supplierbusinessowner_cor_supplier_cor_supplierSupplierId",
                        column: x => x.cor_supplierSupplierId,
                        principalTable: "cor_supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_supplierdocument",
                columns: table => new
                {
                    SupplierDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Document = table.Column<byte[]>(type: "image", nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    cor_supplierSupplierId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_supplierdocument", x => x.SupplierDocumentId);
                    table.ForeignKey(
                        name: "FK_cor_supplierdocument_cor_supplier_cor_supplierSupplierId",
                        column: x => x.cor_supplierSupplierId,
                        principalTable: "cor_supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cor_topsupplier",
                columns: table => new
                {
                    TopSupplierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 50, nullable: true),
                    NoOfStaff = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    cor_supplierSupplierId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_topsupplier", x => x.TopSupplierId);
                    table.ForeignKey(
                        name: "FK_cor_topsupplier_cor_supplier_cor_supplierSupplierId",
                        column: x => x.cor_supplierSupplierId,
                        principalTable: "cor_supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cor_supplier_cor_suppliertypeSupplierTypeId",
                table: "cor_supplier",
                column: "cor_suppliertypeSupplierTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_supplierauthorization_cor_supplierSupplierId",
                table: "cor_supplierauthorization",
                column: "cor_supplierSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_supplierbusinessowner_cor_supplierSupplierId",
                table: "cor_supplierbusinessowner",
                column: "cor_supplierSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_supplierdocument_cor_supplierSupplierId",
                table: "cor_supplierdocument",
                column: "cor_supplierSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_cor_topsupplier_cor_supplierSupplierId",
                table: "cor_topsupplier",
                column: "cor_supplierSupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cor_supplierauthorization");

            migrationBuilder.DropTable(
                name: "cor_supplierbusinessowner");

            migrationBuilder.DropTable(
                name: "cor_supplierdocument");

            migrationBuilder.DropTable(
                name: "cor_topclient");

            migrationBuilder.DropTable(
                name: "cor_topsupplier");

            migrationBuilder.DropTable(
                name: "cor_supplier");

            migrationBuilder.DropTable(
                name: "cor_suppliertype");
        }
    }
}
