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

            modelBuilder.Entity("PPE.DomainObjects.Approval.cor_approvaldetail", b =>
                {
                    b.Property<int>("ApprovalDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("StaffId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("TargetId")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApprovalDetailId");

                    b.ToTable("cor_approvaldetail");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_additionform", b =>
                {
                    b.Property<int>("AdditionFormId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int>("AssetClassificationId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfPurchase")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DepreciationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LpoNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("ResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SubGlAddition")
                        .HasColumnType("int")
                        .HasMaxLength(500);

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsefulLife")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdditionFormId");

                    b.ToTable("ppe_additionform");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_assetclassification", b =>
                {
                    b.Property<int>("AsetClassificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("ClassificationName")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Depreciable")
                        .HasColumnType("bit");

                    b.Property<string>("DepreciationMethod")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<decimal>("ResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SubGlAccumulatedDepreciation")
                        .HasColumnType("int");

                    b.Property<int>("SubGlAddition")
                        .HasColumnType("int");

                    b.Property<int>("SubGlDepreciation")
                        .HasColumnType("int");

                    b.Property<int>("SubGlDisposal")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsefulLifeMax")
                        .HasColumnType("int");

                    b.Property<int>("UsefulLifeMin")
                        .HasColumnType("int");

                    b.HasKey("AsetClassificationId");

                    b.ToTable("ppe_assetclassification");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_reassessment", b =>
                {
                    b.Property<int>("ReassessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AccumulatedDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AssetClassificationId")
                        .HasColumnType("int");

                    b.Property<string>("AssetNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfPurchaase")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("DepreciationForThePeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DepreciationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("LpoNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("NetBookValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProposedUsefulLife")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RemainingUsefulLife")
                        .HasColumnType("int");

                    b.Property<decimal>("ResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsefulLife")
                        .HasColumnType("int");

                    b.HasKey("ReassessmentId");

                    b.ToTable("ppe_reassessment");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_register", b =>
                {
                    b.Property<int>("RegisterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AccumulatedDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AssetClassificationId")
                        .HasColumnType("int");

                    b.Property<string>("AssetNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfPurchaase")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("DepreciationForThePeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DepreciationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("LpoNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("NetBookValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("ResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsefulLife")
                        .HasColumnType("int");

                    b.HasKey("RegisterId");

                    b.ToTable("ppe_register");
                });
#pragma warning restore 612, 618
        }
    }
}
