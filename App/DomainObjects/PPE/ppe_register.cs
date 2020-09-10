using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.DomainObjects.PPE
{
    public partial class ppe_register
    {
        [Key]
        public int RegisterId { get; set; }
        public int AdditionFormId { get; set; }
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
        public int CompanyId { get; set; }
        public int RemainingUsefulLife { get; set; }
        public int ProposedUsefulLife { get; set; }
        public decimal ProposedResidualValue { get; set; }
        public decimal ReEvaluatedCost { get; set; }
        public decimal ResidualValue { get; set; }
        [StringLength(500)]
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public int SubGlAddition { get; set; }
        public int SubGlDepreciation { get; set; }
        public int SubGlAccumulatedDepreciation { get; set; }
        public int SubGlDisposal { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public int ApprovalStatusId { get; set; }
        public string WorkflowToken { get; set; }
        public string RequestDate { get; set; }
        public DateTime ProposedDisposalDate { get; set; }
        public string ReasonForDisposal { get; set; }
        public int Proceed { get; set; }
    }
}
