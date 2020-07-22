using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.DomainObjects.PPE
{
    public class ppe_dailyschedule
    {
        [Key]
        public int PpeDailyScheduleId { get; set; }

        public int? Period { get; set; }

        public decimal? OB { get; set; }

        public decimal? DailyDepreciation { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal DepreciationForThePeriod { get; set; }

        public decimal CB { get; set; }

        public DateTime? PeriodDate { get; set; }

        public int AdditionId { get; set; }

        public int? PeriodId { get; set; }

        public bool? EndPeriod { get; set; }

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
