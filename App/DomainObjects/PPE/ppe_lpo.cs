using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.DomainObjects.PPE
{
    public partial class ppe_lpo
    {
        [Key]
        public int LPOId { get; set; }
        public string LpoNumber { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public bool IsUsed { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
