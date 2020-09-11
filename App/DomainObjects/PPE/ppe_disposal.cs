using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.DomainObjects.PPE
{
    public partial class ppe_disposal
    {
        [Key]
        public int DisposalId { get; set; }
        public int DerecognitionId { get; set; }
        [StringLength(50)]
        public string AssetNumber { get; set; }
        public int AssetClassificationId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public decimal ProceedFromDisposal { get; set; }
        [StringLength(500)]
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
    }
}
