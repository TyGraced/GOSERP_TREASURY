using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPE.Contracts.Response
{
    public class DisposalObj
    {
        public int DisposalId { get; set; }
        public string AssetNumber { get; set; }
        public int AssetClassificationId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfPurchaase { get; set; }
        public int Quantity { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public decimal ProceedFromDisposal { get; set; }
        public string ReasonForDisposal { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ProposedDisposalDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int ApprovalStatusId { get; set; }
        public string WorkflowToken { get; set; }
    }

    public class AddUpdateDisposalObj
    {
        public int DisposalId { get; set; }
        [StringLength(50)]
        public string AssetNumber { get; set; }
        public int AssetClassificationId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfPurchaase { get; set; }
        public int Quantity { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        [StringLength(500)]
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public decimal ProceedFromDisposal { get; set; }
        [StringLength(500)]
        public string ReasonForDisposal { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ProposedDisposalDate { get; set; }
    }

    public class DisposalRegRespObj
    {
        public int DisposalId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DisposalRespObj
    {
        public List<DisposalObj> Disposals { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

}