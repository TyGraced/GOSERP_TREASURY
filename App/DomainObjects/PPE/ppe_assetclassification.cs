using System;
using System.ComponentModel.DataAnnotations;

namespace PPE.DomainObjects.PPE
{
    public partial class ppe_assetclassification
    {
        [Key]
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
    }
}