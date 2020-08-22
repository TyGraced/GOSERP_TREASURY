using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPE.Contracts.Response
{
    public class RegisterObj
    {
        public int RegisterId { get; set; }
        public int AdditionFormId { get; set; }
        public int CompanyId { get; set; }
        public string AssetNumber { get; set; }
        public string LpoNumber { get; set; }
        public string ClassificationName { get; set; }
        public int AssetClassificationId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfPurchaase { get; set; }
        public int Quantity { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public int RemainingUsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public int SubGlAddition { get; set; }
        public string SubGlAdditionName { get; set; }
        public string SubGlAdditionCode { get; set; }
        public int SubGlDepreciation { get; set; }
        public string SubGlDepreciationName { get; set; }
        public string SubGlDepreciationCode { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public string SubGlAccumulatedDepreciationName { get; set; }
        public string SubGlAccumulatedDepreciationCode { get; set; }
        public int SubGlDisposal { get; set; }
        public string SubGlDisposalName { get; set; }
        public string SubGlDisposalCode { get; set; }
        public int ProposedUsefulLife { get; set; }
        public int ProposedResidualValue { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class AddUpdateRegisterObj
    {
        public int RegisterId { get; set; }
        public int AdditionFormId { get; set; }
        public int CompanyId { get; set; }
        [StringLength(50)]
        public string AssetNumber { get; set; }
        [StringLength(50)]
        public string LpoNumber { get; set; }
        public string ClassificationName { get; set; }
        public int AssetClassificationId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfPurchaase { get; set; }
        public int Quantity { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public int RemainingUsefulLife { get; set; }
        public int ProposedUsefulLife { get; set; }
        public int ProposedResidualValue { get; set; }
        public decimal ResidualValue { get; set; }
        [StringLength(50)]
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public int SubGlAddition { get; set; }
        public string SubGlAdditionName { get; set; }
        public string SubGlAdditionCode { get; set; }
        public int SubGlDepreciation { get; set; }
        public string SubGlDepreciationName { get; set; }
        public string SubGlDepreciationCode { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public string SubGlAccumulatedDepreciationName { get; set; }
        public string SubGlAccumulatedDepreciationCode { get; set; }
        public int SubGlDisposal { get; set; }
        public string SubGlDisposalName { get; set; }
        public string SubGlDisposalCode { get; set; }
    }

    public class RegisterRegRespObj
    {
        public int RegisterId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class RegisterRespObj
    {
        public List<RegisterObj> Registers { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
