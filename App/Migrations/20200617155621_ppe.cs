using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPE.Migrations
{
    public partial class ppe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cor_serviceterms");

            migrationBuilder.DropTable(
                name: "cor_supplierauthorization");

            migrationBuilder.DropTable(
                name: "cor_supplierbusinessowner");

            migrationBuilder.DropTable(
                name: "cor_supplierdocument");

            migrationBuilder.DropTable(
                name: "cor_tasksetup");

            migrationBuilder.DropTable(
                name: "cor_topclient");

            migrationBuilder.DropTable(
                name: "cor_topsupplier");

            migrationBuilder.DropTable(
                name: "cor_supplier");

            migrationBuilder.DropTable(
                name: "cor_suppliertype");

            migrationBuilder.CreateTable(
                name: "ppe_additionform",
                columns: table => new
                {
                    AdditionFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LpoNumber = table.Column<string>(maxLength: 50, nullable: true),
                    DateOfPurchase = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    AssetClassificationId = table.Column<int>(nullable: false),
                    SubGlAddition = table.Column<int>(maxLength: 500, nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(nullable: false),
                    UsefulLife = table.Column<int>(nullable: false),
                    ResidualValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_additionform", x => x.AdditionFormId);
                });

            migrationBuilder.CreateTable(
                name: "ppe_assetclassification",
                columns: table => new
                {
                    AsetClassificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassificationName = table.Column<string>(maxLength: 500, nullable: true),
                    UsefulLifeMin = table.Column<int>(nullable: false),
                    UsefulLifeMax = table.Column<int>(nullable: false),
                    ResidualValue = table.Column<decimal>(nullable: false),
                    Depreciable = table.Column<bool>(nullable: false),
                    DepreciationMethod = table.Column<string>(maxLength: 500, nullable: true),
                    SubGlAddition = table.Column<int>(nullable: false),
                    SubGlDepreciation = table.Column<int>(nullable: false),
                    SubGlAccumulatedDepreciation = table.Column<int>(nullable: false),
                    SubGlDisposal = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ppe_assetclassification", x => x.AsetClassificationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ppe_additionform");

            migrationBuilder.DropTable(
                name: "ppe_assetclassification");

            migrationBuilder.CreateTable(
                name: "cor_serviceterms",
                columns: table => new
                {
                    ServiceTermsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_serviceterms", x => x.ServiceTermsId);
                });

            migrationBuilder.CreateTable(
                name: "cor_suppliertype",
                columns: table => new
                {
                    SupplierTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    GL = table.Column<int>(type: "int", nullable: false),
                    SupplierTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_suppliertype", x => x.SupplierTypeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_tasksetup",
                columns: table => new
                {
                    TaskSetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    SubGL = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_tasksetup", x => x.TaskSetupId);
                });

            migrationBuilder.CreateTable(
                name: "cor_topclient",
                columns: table => new
                {
                    TopClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NoOfStaff = table.Column<int>(type: "int", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_topclient", x => x.TopClientId);
                });

            migrationBuilder.CreateTable(
                name: "cor_supplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HaveWorkPrintPermit = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SupplierNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierTypeId = table.Column<int>(type: "int", nullable: false),
                    TaxIDorVATID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cor_suppliertypeSupplierTypeId = table.Column<int>(type: "int", nullable: true)
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
                    SupplierAuthorizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cor_supplierSupplierId = table.Column<int>(type: "int", nullable: true)
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
                    SupplierBusinessOwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cor_supplierSupplierId = table.Column<int>(type: "int", nullable: true)
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
                    SupplierDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Document = table.Column<byte[]>(type: "image", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cor_supplierSupplierId = table.Column<int>(type: "int", nullable: true)
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
                    TopSupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NoOfStaff = table.Column<int>(type: "int", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cor_supplierSupplierId = table.Column<int>(type: "int", nullable: true)
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
    }
}
