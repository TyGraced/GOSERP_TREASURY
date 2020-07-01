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
        [StringLength(500)]
        public string Location { get; set; }
        public decimal DepreciationForThePeriod { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
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
