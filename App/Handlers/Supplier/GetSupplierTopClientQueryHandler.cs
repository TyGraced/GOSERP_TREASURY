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
    public class GetSupplierTopClientQueryHandler : IRequestHandler<GetSupplierTopClientQuery, SupplierTopClientRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper;
        public GetSupplierTopClientQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supRepo = supplierRepository;
        }
        public async Task<SupplierTopClientRespObj> Handle(GetSupplierTopClientQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supRepo.GetSupplierBusinessOwnerAsync(request.ClientTopId);
            return new SupplierTopClientRespObj
            {
                SupplierTopClients = _mapper.Map<List<SupplierTopClientObj>>(supplier),
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
