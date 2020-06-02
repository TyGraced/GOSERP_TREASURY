using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Queries
{
    public class GetAllSupplierQuery : IRequest<SupplierRespObj> { }
    public class GetSupplierQuery : IRequest<SupplierRespObj>
    {
        public GetSupplierQuery() { }
        public int SupplierId { get; set; }
        public GetSupplierQuery(int supplierId)
        {
            SupplierId = supplierId;
        }
    }
}
