using MediatR;
using Puchase_and_payables.Contracts.Response.Supplier; 

namespace Puchase_and_payables.Contracts.Queries.Supplier
{
    public class GetTaskSetupQuery : IRequest<TasksetupRespObj>
    {
        public GetTaskSetupQuery() { }
        public int TasksetupId { get; set; }
        public GetTaskSetupQuery(int tasksetupId)
        {
            TasksetupId = tasksetupId;
        }
    }
    public class GetServiceTermsQuery : IRequest<ServiceTermRespObj>
    {
        public GetServiceTermsQuery() { }
        public int ServiceTermId { get; set; }
        public GetServiceTermsQuery(int serviceTermId)
        {
            ServiceTermId = serviceTermId;
        }
    }
    public class GetSupplierTypeQuery : IRequest<SuppliertypeRespObj>
    {
        public GetSupplierTypeQuery() { }
        public int SupplierTypeId { get; set; }
        public GetSupplierTypeQuery(int supplierTypeId)
        {
            SupplierTypeId = supplierTypeId;
        }
    }

    public class GetAllTaskSetupQuery : IRequest<TasksetupRespObj> { }
    public class GetAllServiceTermsQuery : IRequest<ServiceTermRespObj> { }
    public class GetAllSupplierTypeQuery : IRequest<SuppliertypeRespObj> { }
}
