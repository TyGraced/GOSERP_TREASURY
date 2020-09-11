﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPE.Data;

namespace PPE.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200911104126_drecognition")]
    partial class drecognition
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("CompanyId")
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

                    b.Property<int>("SubGlAccumulatedDepreciation")
                        .HasColumnType("int");

                    b.Property<int>("SubGlAddition")
                        .HasColumnType("int")
                        .HasMaxLength(500);

                    b.Property<int>("SubGlDepreciation")
                        .HasColumnType("int");

                    b.Property<int>("SubGlDisposal")
                        .HasColumnType("int");

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

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_dailyschedule", b =>
                {
                    b.Property<int>("PpeDailyScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AccumulatedDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AdditionId")
                        .HasColumnType("int");

                    b.Property<decimal>("CB")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("Createdon")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DailyDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("DepreciationForThePeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("EndPeriod")
                        .HasColumnType("bit");

                    b.Property<decimal?>("OB")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Period")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PeriodDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PeriodId")
                        .HasColumnType("int");

                    b.Property<int>("RegisterId")
                        .HasColumnType("int");

                    b.Property<string>("Updatedby")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("Updatedon")
                        .HasColumnType("datetime2");

                    b.HasKey("PpeDailyScheduleId");

                    b.ToTable("ppe_dailyschedule");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_derecognition", b =>
                {
                    b.Property<int>("DerecognitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("Createdon")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("NBV")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ProposedDisposalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReasonForDisposal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Updatedby")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("Updatedon")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DerecognitionId");

                    b.ToTable("ppe_derecognition");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_disposal", b =>
                {
                    b.Property<int>("DisposalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AccumulatedDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit")
                        .HasMaxLength(500);

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

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

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DepreciationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DerecognitionId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("NetBookValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProceedFromDisposal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DisposalId");

                    b.ToTable("ppe_disposal");
                });

            modelBuilder.Entity("PPE.DomainObjects.PPE.ppe_lpo", b =>
                {
                    b.Property<int>("PLPOId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(550)")
                        .HasMaxLength(550);

                    b.Property<decimal>("AmountPayable")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int>("BidAndTenderId")
                        .HasColumnType("int");

                    b.Property<bool>("BidComplete")
                        .HasColumnType("bit");

                    b.Property<int>("DebitGl")
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<int>("JobStatus")
                        .HasColumnType("int");

                    b.Property<string>("LPONumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("PurchaseReqNoteId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ServiceTerm")
                        .HasColumnType("int");

                    b.Property<string>("SupplierAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Tax")
                        .HasColumnType("money");

                    b.Property<string>("Taxes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("money");

                    b.Property<int>("WinnerSupplierId")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PLPOId");

                    b.ToTable("ppe_lpo");
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

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

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

                    b.Property<int>("ProposedResidualValue")
                        .HasColumnType("int");

                    b.Property<int>("ProposedUsefulLife")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RemainingUsefulLife")
                        .HasColumnType("int");

                    b.Property<decimal>("ResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SubGlAccumulatedDepreciation")
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

                    b.Property<int>("UsefulLife")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("AdditionFormId")
                        .HasColumnType("int");

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int>("AssetClassificationId")
                        .HasColumnType("int");

                    b.Property<string>("AssetNumber")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

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

                    b.Property<decimal>("ProposedResidualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProposedUsefulLife")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("ReEvaluatedCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RemainingUsefulLife")
                        .HasColumnType("int");

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

                    b.Property<int>("UsefulLife")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RegisterId");

                    b.ToTable("ppe_register");
                });
#pragma warning restore 612, 618
        }
    }
}
