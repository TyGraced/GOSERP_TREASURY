﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPE.Data;

namespace PPE.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HaveWorkPrintPermit")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Passport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PostalAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistrationNo")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("SupplierNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SupplierTypeId")
                        .HasColumnType("int");

                    b.Property<string>("TaxIDorVATID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("cor_suppliertypeSupplierTypeId")
                        .HasColumnType("int");

                    b.HasKey("SupplierId");

                    b.HasIndex("cor_suppliertypeSupplierTypeId");

                    b.ToTable("cor_supplier");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierauthorization", b =>
                {
                    b.Property<int>("SupplierAuthorizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Signature")
                        .HasColumnType("image");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("cor_supplierSupplierId")
                        .HasColumnType("int");

                    b.HasKey("SupplierAuthorizationId");

                    b.HasIndex("cor_supplierSupplierId");

                    b.ToTable("cor_supplierauthorization");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierbusinessowner", b =>
                {
                    b.Property<int>("SupplierBusinessOwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Signature")
                        .HasColumnType("image");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("cor_supplierSupplierId")
                        .HasColumnType("int");

                    b.HasKey("SupplierBusinessOwnerId");

                    b.HasIndex("cor_supplierSupplierId");

                    b.ToTable("cor_supplierbusinessowner");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierdocument", b =>
                {
                    b.Property<int>("SupplierDocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Document")
                        .HasColumnType("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("cor_supplierSupplierId")
                        .HasColumnType("int");

                    b.HasKey("SupplierDocumentId");

                    b.HasIndex("cor_supplierSupplierId");

                    b.ToTable("cor_supplierdocument");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_suppliertype", b =>
                {
                    b.Property<int>("SupplierTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("GL")
                        .HasColumnType("int");

                    b.Property<string>("SupplierTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("SupplierTypeId");

                    b.ToTable("cor_suppliertype");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_topclient", b =>
                {
                    b.Property<int>("TopClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<string>("ContactPerson")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("NoOfStaff")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("TopClientId");

                    b.ToTable("cor_topclient");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_topsupplier", b =>
                {
                    b.Property<int>("TopSupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<string>("ContactPerson")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("NoOfStaff")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("cor_supplierSupplierId")
                        .HasColumnType("int");

                    b.HasKey("TopSupplierId");

                    b.HasIndex("cor_supplierSupplierId");

                    b.ToTable("cor_topsupplier");
                });

            modelBuilder.Entity("PPE.DomainObjects.Supplier.cor_serviceterms", b =>
                {
                    b.Property<int>("ServiceTermsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Header")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("ServiceTermsId");

                    b.ToTable("cor_serviceterms");
                });

            modelBuilder.Entity("PPE.DomainObjects.Supplier.cor_tasksetup", b =>
                {
                    b.Property<int>("TaskSetupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<int>("SubGL")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("TaskSetupId");

                    b.ToTable("cor_tasksetup");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", b =>
                {
                    b.HasOne("GODP.APIsContinuation.DomainObjects.Supplier.cor_suppliertype", "cor_suppliertype")
                        .WithMany("cor_supplier")
                        .HasForeignKey("cor_suppliertypeSupplierTypeId");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierauthorization", b =>
                {
                    b.HasOne("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", "cor_supplier")
                        .WithMany("cor_supplierauthorization")
                        .HasForeignKey("cor_supplierSupplierId");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierbusinessowner", b =>
                {
                    b.HasOne("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", "cor_supplier")
                        .WithMany("cor_supplierbusinessowner")
                        .HasForeignKey("cor_supplierSupplierId");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplierdocument", b =>
                {
                    b.HasOne("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", "cor_supplier")
                        .WithMany("cor_supplierdocument")
                        .HasForeignKey("cor_supplierSupplierId");
                });

            modelBuilder.Entity("GODP.APIsContinuation.DomainObjects.Supplier.cor_topsupplier", b =>
                {
                    b.HasOne("GODP.APIsContinuation.DomainObjects.Supplier.cor_supplier", "cor_supplier")
                        .WithMany("cor_topsupplier")
                        .HasForeignKey("cor_supplierSupplierId");
                });
#pragma warning restore 612, 618
        }
    }
}
