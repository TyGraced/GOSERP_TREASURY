using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TREASURY.DomainObjects.Approval
{

    public class cor_approvaldetail
    {
        [Key]
        public int ApprovalDetailId { get; set; }
        public int StatusId { get; set; }
        public int StaffId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
    }
}
