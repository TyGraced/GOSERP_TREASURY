using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.APIsContinuation.DomainObjects.Supplier
{
    public partial class cor_suppliertype
    {
        public cor_suppliertype()
        {
            cor_supplier = new HashSet<cor_supplier>();
        }

        [Key]
        public int SupplierTypeId { get; set; }

        [Required]
        [StringLength(250)]
        public string SupplierTypeName { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<cor_supplier> cor_supplier { get; set; }
    }
}
