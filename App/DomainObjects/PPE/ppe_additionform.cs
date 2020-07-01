using System;
using System.ComponentModel.DataAnnotations;

namespace PPE.DomainObjects.PPE
{
    public partial class ppe_additionform
    {
        [Key]
        public int AdditionFormId { get; set; }
        [StringLength(50)]
        public string LpoNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public int AssetClassificationId { get; set; }
        [StringLength(500)]
        public int SubGlAddition { get; set; }
        public DateTime DepreciationStartDate { get; set; }
        public int UsefulLife { get; set; }
        public decimal ResidualValue { get; set; }
        public string Location { get; set; }
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