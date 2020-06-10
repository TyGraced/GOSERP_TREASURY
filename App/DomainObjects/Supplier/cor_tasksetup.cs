using Puchase_and_payables.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Puchase_and_payables.DomainObjects.Supplier
{
    public class cor_tasksetup : GeneralEntity
    {
        [Key]
        public int TaskSetupId { get; set; }
        public double Percentage { get; set; }
        public string Type { get; set; }
        public int SubGL { get; set; }
    }
}
