using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using Puchase_and_payables.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Supplier
{
    public class UpdateSupplierDocumentCommand : IRequest<SupplierDocumentRegRespObj>
    {
        public int SupplierDocumentId { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public byte[] Document { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class DeleteSupplierDocumentCommand : IRequest<DeleteRespObj>
    {
        public List<DeleteItemReqObj> req { get; set; }
    }
}
