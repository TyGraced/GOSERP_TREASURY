using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.DomainObjects.PPE
{
    public class ppe_derecognition
    {
        [Key]
        public int DerecognitionId { get; set; }

        public string ReasonForDisposal { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ProposedDisposalDate { get; set; }
        public int ApprovalStatusId { get; set; }
        public decimal NBV { get; set; }
        public string WorkflowToken { get; set; }
        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        public DateTime? Updatedon { get; set; }

        public DateTime? Createdon { get; set; }
    }
}
