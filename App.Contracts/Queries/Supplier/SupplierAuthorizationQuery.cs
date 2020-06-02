using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Queries
{
    public class GetAllSupplierAuthorizationQuery : IRequest<SupplierAuthorizationRespObj> { }

    public class GetSupplierAuthorization : IRequest<SupplierAuthorizationRespObj>
    {
        public GetSupplierAuthorization() { }
        public int SupplierAuthorizationId { get; set; }
        public GetSupplierAuthorization(int supplierAuthorizationId)
        {
            SupplierAuthorizationId = supplierAuthorizationId;
        }
    }
}
