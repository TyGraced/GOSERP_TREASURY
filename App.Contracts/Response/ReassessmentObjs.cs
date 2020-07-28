using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPE.Contracts.Response
{
    public class ReassessmentObj
    {
        public int ReassessmentId { get; set; }
        public string AssetNumber { get; set; }
        public string LpoNumber { get; set; }
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
        public int RemainingUsefulLife { get; set; }
        public int ProposedUsefulLife { get; set; }
        public int SubGlDepreciation { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public int SubGlDisposal { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class AddUpdateReassessmentObj
    {
        public int ReassessmentId { get; set; }
        [StringLength(50)]
        public string AssetNumber { get; set; }
        [StringLength(50)]
        public string LpoNumber { get; set; }
        public int AssetClassificationId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfPurchaase { get; set; }
        public int Quantity { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        [StringLength(50)]
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public int RemainingUsefulLife { get; set; }
        public int ProposedUsefulLife { get; set; }
    }

    public class ReassessmentRegRespObj
    {
        public int ReassessmentId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ReassessmentRespObj
    {
        public List<ReassessmentObj> Reassessments { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
