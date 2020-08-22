using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPE.Contracts.Response
{
    public class LpoObj
    {
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

    public class LpoRegRespObj
    {
        public int LPOId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LpoRespObj
    {
        public List<LpoObj> lpos { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
