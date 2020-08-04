using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPE.Contracts.Response
{
    public class AssetClassificationObj
    {
        public int AsetClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public int UsefulLifeMin { get; set; }
        public int UsefulLifeMax { get; set; }
        public decimal ResidualValue { get; set; }
        public bool Depreciable { get; set; }
        public string DepreciationMethod { get; set; }
        public int SubGlAddition { get; set; }
        public string SubGlAdditionName { get; set; }
        public int SubGlDepreciation { get; set; }
        public string SubGlDepreciationName { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public string SubGlAccumulatedDepreciationName { get; set; }
        public int SubGlDisposal { get; set; }
        public string SubGlDisposalName { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class AddUpdateAssetClassificationObj
    {
        public int AsetClassificationId { get; set; }
        [StringLength(500)]
        public string ClassificationName { get; set; }
        public int UsefulLifeMin { get; set; }
        public int UsefulLifeMax { get; set; }
        public decimal ResidualValue { get; set; }
        public bool Depreciable { get; set; }
        [StringLength(500)]
        public string DepreciationMethod { get; set; }
        public int SubGlAddition { get; set; }
        public string SubGlAdditionName { get; set; }
        public int SubGlDepreciation { get; set; }
        public string SubGlDepreciationName { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public string SubGlAccumulatedDepreciationName { get; set; }
        public int SubGlDisposal { get; set; }
        public string SubGlDisposalName { get; set; }
    }

    public class AssetClassificationRegRespObj
    {
        public int AsetClassificationId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AssetClassificationRespObj
    {
        public List<AssetClassificationObj> AssetClassifications { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SearchObj
    {
        public int SearchId { get; set; }
        public string SearchWord { get; set; }
    }

    public class DeleteRequest
    {
        public List<int> ItemIds { get; set; }
    }

    public class DeleteRespObjt
    {
        public bool Deleted { get; set; }
        public APIResponseStatus Status { get; set; }
    }


}