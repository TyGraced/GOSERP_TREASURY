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

    public class GetSupplierTypeQueryHandler : IRequestHandler<GetSupplierTypeQuery, SuppliertypeRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        public GetSupplierTypeQueryHandler(ISupplierRepository supplierRepository)
        {
            _supRepo = supplierRepository;
        }
        public async Task<SuppliertypeRespObj> Handle(GetSupplierTypeQuery request, CancellationToken cancellationToken)
        {
            var item = await _supRepo.GetSupplierTypeAsync(request.SupplierTypeId);

            var respList = new List<SuppliertypeObj>();
            if (item != null)
            {
                var respItem = new SuppliertypeObj
                {
                    Active = item.Active,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    SupplierTypeId = item.SupplierTypeId,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn,
                    GL = item.GL,
                    SupplierTypeName = item.SupplierTypeName,
                    
                };
                respList.Add(respItem);
            }

            return new SuppliertypeRespObj
            {
                Suppliertypes = respList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = respList.Count() > 0 ? null : "Search Complete!! No Record Found"
                    }
                }
            };
        }
    }
}
