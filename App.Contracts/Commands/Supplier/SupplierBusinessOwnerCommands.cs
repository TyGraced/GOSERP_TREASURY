using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using Puchase_and_payables.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Supplier
{
    public class UpdateSupplierBuisnessOwnerCommand : IRequest<SupplierBuisnessOwnerRegRespObj>
    {
        public int SupplierBusinessOwnerId { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Signature { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class DeleteSupplierBuisnessOwnerCommand : IRequest<DeleteRespObj>
    {
        public List<DeleteItemReqObj> req { get; set; }
    }
}
