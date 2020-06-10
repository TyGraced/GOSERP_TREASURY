using AutoMapper;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Queries; 
using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.GOS_API_Response;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class GetSupplierIinformationQueryHandler : IRequestHandler<GetSupplierQuery, SupplierRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper;
        public GetSupplierIinformationQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supRepo = supplierRepository;
        }
        public async Task<SupplierRespObj> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var supplier= await _supRepo.GetSupplierAsync(request.SupplierId);
            return new SupplierRespObj
            {
                Suppliers = _mapper.Map<List<SupplierObj>>(supplier),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = supplier == null ? "Search Complete!! No Record Found" : null
                    }
                }
            };
        }
    }
}
