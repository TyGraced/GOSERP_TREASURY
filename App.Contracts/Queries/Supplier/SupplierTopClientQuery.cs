using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Queries
{
    public class GetAllSupplierTopClientQuery : IRequest<SupplierTopClientRespObj> { }
    public class GetSupplierTopClientQuery : IRequest<SupplierTopClientRespObj>
    {
        public GetSupplierTopClientQuery() { }
        public int ClientTopId { get; set; }
        public GetSupplierTopClientQuery(int clientTopId)
        {
            ClientTopId = clientTopId;
        }
    }
}
