using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GODP.APIsContinuation.DomainObjects.Supplier
{
    public partial class cor_supplierdocument
    {
        [Key]
        public int SupplierDocumentId { get; set; }

        public int SupplierId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        public byte[] Document { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual cor_supplier cor_supplier { get; set; }
    }
}
