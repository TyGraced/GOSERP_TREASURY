using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Puchase_and_payables.Contracts.Queries.Supplier;
using Puchase_and_payables.Contracts.Response.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{

    public class GetAllSupplierTypeQueryHandler : IRequestHandler<GetAllSupplierTypeQuery, SuppliertypeRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        public GetAllSupplierTypeQueryHandler(ISupplierRepository supplierRepository)
        {
            _supRepo = supplierRepository;
        }
        public async Task<SuppliertypeRespObj> Handle(GetAllSupplierTypeQuery request, CancellationToken cancellationToken)
        {
            var list = await _supRepo.GetAllSupplierTypeAsync();


            return new SuppliertypeRespObj
            {
                Suppliertypes = list.Select(x => new SuppliertypeObj()
                {
                    UpdatedOn = x.UpdatedOn,
                    Active = x.Active,
                    CreatedBy = x.CreatedBy ?? string.Empty,
                    CreatedOn = x.CreatedOn,
                    Deleted = x.Deleted, 
                    UpdatedBy = x.UpdatedBy, 
                    GL = x.GL,
                    SupplierTypeName = x.SupplierTypeName,
                    SupplierTypeId = x.SupplierTypeId
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete!! No Record Found"
                    }
                }
            };
        }
    }
}
