using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPE.Contracts.Response
{
    public class AdditionFormObj
    {
        public int AdditionFormId { get; set; }
        public string LpoNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public int AssetClassificationId { get; set; }
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
        public string ClassificationName { get; set; }
        public string SubGlDisposalCode { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class AddUpdateAdditionFormObj
    {
        public int AdditionFormId { get; set; }
        public string LpoNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string Description { get; set; }
        public string SubGlName { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public int AssetClassificationId { get; set; }
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
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        public string Location { get; set; }
    }

    public class AdditionFormRegRespObj
    {
        public int AdditionFormId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AdditionFormRespObj
    {
        public List<AdditionFormObj> AdditionForms { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}